using HabitTracker.Application.DTOs;
using HabitTracker.Domain.Entities;
using HabitTracker.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HabitTracker.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class AnalyticsController : ControllerBase
    {
        private readonly IAnalyticsRepository _analyticsRepository;
        private readonly IHabitRepository _habitRepository;

        public AnalyticsController(IAnalyticsRepository analyticsRepository, IHabitRepository habitRepository)
        {
            _analyticsRepository = analyticsRepository;
            _habitRepository = habitRepository;
        }

        // Helper method to extract the current user's ID from token claims.
        private bool TryGetUserId(out int userId)
        {
            userId = 0;
            var userIdClaim = HttpContext.User.Claims.FirstOrDefault(c =>
                c.Type == ClaimTypes.NameIdentifier || c.Type == "sub");
            if (userIdClaim == null) return false;
            return int.TryParse(userIdClaim.Value, out userId);
        }

        // ✅ GET: api/Analytics/user (Includes HabitName)
        [HttpGet("user")]
        public async Task<IActionResult> GetUserAnalytics()
        {
            if (!TryGetUserId(out int userId))
                return Unauthorized("User ID not found in token.");

            var analyticsData = await _analyticsRepository.GetUserAnalyticsAsync(userId);

            // ✅ Fetch Habit Names separately
            var habitIds = analyticsData.Select(a => a.HabitId).Distinct();
            var habits = await _habitRepository.GetHabitsByIdsAsync(habitIds);

            var analyticsDtoList = analyticsData.Select(a => new AnalyticsDTO
            {
                Id = a.Id,
                UserId = a.UserId,
                HabitId = a.HabitId,
                ProgressData = a.ProgressData,
                LastUpdated = a.LastUpdated,
                HabitName = habits.ContainsKey(a.HabitId) ? habits[a.HabitId] : "Unknown Habit"
            }).ToList();

            return Ok(analyticsDtoList);
        }

        // ✅ GET: api/Analytics/habit/{habitId}
        [HttpGet("habit/{habitId}")]
        public async Task<IActionResult> GetAnalyticsByHabit(int habitId)
        {
            var analytics = await _analyticsRepository.GetAnalyticsByHabitAsync(habitId);
            if (analytics == null)
                return NotFound();

            var habit = await _habitRepository.GetHabitByIdAsync(habitId);
            var analyticsDto = new AnalyticsDTO
            {
                Id = analytics.Id,
                UserId = analytics.UserId,
                HabitId = analytics.HabitId,
                ProgressData = analytics.ProgressData,
                LastUpdated = analytics.LastUpdated,
                HabitName = habit?.Title ?? "Unknown Habit"
            };

            return Ok(analyticsDto);
        }

        // ✅ POST: api/Analytics/update
        [HttpPost("update")]
        public async Task<IActionResult> UpdateAnalytics([FromBody] UpdateAnalyticsDTO updateDto)
        {
            if (updateDto == null)
                return BadRequest();

            if (!TryGetUserId(out int userId))
                return Unauthorized("User ID not found in token.");

            updateDto.UserId = userId;

            var analytics = new Analytics
            {
                UserId = updateDto.UserId,
                HabitId = updateDto.HabitId,
                ProgressData = updateDto.ProgressData,
                LastUpdated = DateTime.UtcNow
            };

            await _analyticsRepository.AddOrUpdateAnalyticsAsync(analytics);
            return Ok(analytics);
        }
    }
}
