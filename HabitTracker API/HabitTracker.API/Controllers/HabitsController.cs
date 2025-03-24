using HabitTracker.Application.DTOs;
using HabitTracker.Domain.Entities;
using HabitTracker.Domain.Interfaces;
using HabitTracker.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

namespace HabitTracker.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class HabitsController : ControllerBase
    {
        private readonly IHabitRepository _habitRepository;
        private readonly IHabitCompletionRepository _habitCompletionRepository;
        private readonly IAnalyticsRepository _analyticsRepository;
        private readonly IGamificationService _gamificationService;

        public HabitsController(
            IHabitRepository habitRepository,
            IHabitCompletionRepository habitCompletionRepository,
            IAnalyticsRepository analyticsRepository,
            IGamificationService gamificationService)
        {
            _habitRepository = habitRepository;
            _habitCompletionRepository = habitCompletionRepository;
            _analyticsRepository = analyticsRepository;
            _gamificationService = gamificationService;
        }

        // ✅ GET: api/Habits/user
        [HttpGet("user")]
        public async Task<IActionResult> GetUserHabits()
        {
            if (!TryGetUserId(out int userId))
                return Unauthorized("User ID not found in token.");

            var habits = await _habitRepository.GetUserHabitsAsync(userId);
            return Ok(habits);
        }

        // ✅ GET: api/Habits/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetHabit(int id)
        {
            var habit = await _habitRepository.GetHabitByIdAsync(id);
            return habit == null ? NotFound() : Ok(habit);
        }

        // ✅ POST: api/Habits/create
        [HttpPost("create")]
        public async Task<IActionResult> CreateHabit([FromBody] CreateHabitDTO createDto)
        {
            if (!TryGetUserId(out int userId))
                return Unauthorized("User ID not found in token.");

            var habit = new Habit
            {
                UserId = userId,
                Title = createDto.Title,
                Description = createDto.Description,
                Frequency = createDto.Frequency,
                StartDate = createDto.StartDate,
                EndDate = createDto.EndDate,
                ReminderTime = createDto.ReminderTime
            };

            await _habitRepository.AddHabitAsync(habit);
            return Ok(habit);
        }

        // ✅ DELETE: api/Habits/delete/{id}
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteHabit(int id)
        {
            var habit = await _habitRepository.GetHabitByIdAsync(id);
            if (habit == null)
                return NotFound();

            await _habitRepository.DeleteHabitAsync(id);
            return Ok();
        }

        // ✅ POST: api/Habits/mark-completion
        // POST: api/Habits/mark-completion
        [HttpPost("mark-completion")]
        public async Task<IActionResult> MarkHabitCompletion([FromBody] MarkHabitDTO dto)
        {
            // Extract user ID from token
            var userIdClaim = HttpContext.User.Claims.FirstOrDefault(c =>
                c.Type == ClaimTypes.NameIdentifier || c.Type == "sub");
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
                return Unauthorized("User ID not found in token.");

            dto.UserId = userId;
            var today = DateTime.UtcNow.Date;

            var existing = await _habitCompletionRepository.GetHabitCompletionAsync(dto.HabitId, userId, today);
            if (existing != null)
            {
                existing.IsCompleted = dto.IsCompleted;
                await _habitCompletionRepository.UpdateHabitCompletionAsync(existing);
            }
            else
            {
                var completion = new HabitCompletion
                {
                    HabitId = dto.HabitId,
                    UserId = userId,
                    CompletedDate = today,
                    IsCompleted = dto.IsCompleted
                };
                await _habitCompletionRepository.AddHabitCompletionAsync(completion);
            }

            // ✅ Update analytics when habit is marked complete
            if (dto.IsCompleted)
            {
                // Retrieve existing analytics record for the habit
                var existingAnalytics = await _analyticsRepository.GetAnalyticsByHabitAsync(dto.HabitId);

                int completedDays = 0;
                int missedDays = 0;
                DateTime lastUpdated = DateTime.MinValue;

                if (existingAnalytics != null)
                {
                    // Parse existing progress data
                    var progress = JsonSerializer.Deserialize<Dictionary<string, int>>(existingAnalytics.ProgressData);
                    completedDays = progress.ContainsKey("completedDays") ? progress["completedDays"] : 0;
                    missedDays = progress.ContainsKey("missedDays") ? progress["missedDays"] : 0;

                    // Get the last recorded update time
                    lastUpdated = existingAnalytics.LastUpdated;
                }

                // ✅ Calculate Missed Days
                if (lastUpdated != DateTime.MinValue)
                {
                    int daysSinceLastUpdate = (today - lastUpdated.Date).Days;
                    if (daysSinceLastUpdate > 1)
                    {
                        missedDays += daysSinceLastUpdate - 1;  // Count only days *before* today
                    }
                }

                // ✅ Increase completed days count
                completedDays++;

                // ✅ Store updated progress data
                var updatedProgressData = new Dictionary<string, int>
        {
            { "completedDays", completedDays },
            { "missedDays", missedDays }
        };

                // ✅ Update analytics table with new progress
                await _analyticsRepository.AddOrUpdateAnalyticsAsync(new Analytics
                {
                    UserId = userId,
                    HabitId = dto.HabitId,
                    ProgressData = JsonSerializer.Serialize(updatedProgressData), // ✅ Store as JSON
                    LastUpdated = today // ✅ Update last tracked date
                });

                // ✅ Reward XP & Check for Badges
                var xpEarned = 10; // Adjust XP reward as needed
                var updatedGamification = await _gamificationService.RewardXP(userId, xpEarned);
            }

            return Ok(new { Message = "Habit completion recorded, analytics updated, XP rewarded, and badges checked." });
        }



        [HttpGet("today-completion/{habitId}")]
        public async Task<IActionResult> GetTodayCompletion(int habitId)
        {
            var userIdClaim = HttpContext.User.Claims.FirstOrDefault(c =>
                c.Type == ClaimTypes.NameIdentifier || c.Type == "sub");
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
                return Unauthorized("User ID not found in token.");

            var today = DateTime.UtcNow.Date;
            var completion = await _habitCompletionRepository.GetHabitCompletionAsync(habitId, userId, today);
            if (completion == null)
            {
                return Ok(new { IsCompleted = false });
            }
            return Ok(new { completion.IsCompleted });
        }

        private bool TryGetUserId(out int userId)
        {
            userId = 0;
            var userIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier || c.Type == "sub");
            return userIdClaim != null && int.TryParse(userIdClaim.Value, out userId);
        }
    }
}
