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
    public class GamificationController : ControllerBase
    {
        private readonly IUserGamificationRepository _userGamificationRepository;
        private readonly IBadgeRepository _badgeRepository;
        private readonly IUserBadgeRepository _userBadgeRepository;
        private readonly IUserRepository _userRepository; // ✅ Use IUserRepository


        public GamificationController(
            IUserGamificationRepository userGamificationRepository,
            IBadgeRepository badgeRepository,
            IUserRepository userRepository,
            IUserBadgeRepository userBadgeRepository)
            {
                _userGamificationRepository = userGamificationRepository;
                _badgeRepository = badgeRepository;
                _userRepository = userRepository;
                _userBadgeRepository = userBadgeRepository;
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

        // GET: api/Gamification/user
        [HttpGet("user")]
        public async Task<IActionResult> GetUserGamification()
        {
            if (!TryGetUserId(out int userId))
                return Unauthorized("User ID not found in token.");

            var gamification = await _userGamificationRepository.GetByUserIdAsync(userId);
            if (gamification == null)
                return NotFound("User gamification data not found.");

            var user = await _userRepository.GetUserByIdAsync(userId); // ✅ Fetch user from IUserRepository

            var userGamificationDto = new UserGamificationDTO
            {
                Id = gamification.Id,
                UserId = gamification.UserId,
                XP = gamification.XP,
                Level = gamification.Level,
                UserName = user?.FullName ?? "Unknown User" // ✅ Use IUserRepository
            };

            return Ok(userGamificationDto);
        }


        // GET: api/Gamification/badges
        [HttpGet("badges")]
        public async Task<IActionResult> GetAllBadges()
        {
            var badges = await _badgeRepository.GetAllBadgesAsync();
            return Ok(badges);
        }

        // GET: api/Gamification/user-badges
        [HttpGet("user-badges")]
        public async Task<IActionResult> GetUserBadges()
        {
            if (!TryGetUserId(out int userId))
                return Unauthorized("User ID not found in token.");

            var userBadges = await _userBadgeRepository.GetUserBadgesAsync(userId);

            if (userBadges == null || !userBadges.Any())
                return Ok(new List<object>());

            // Fetch Badge details and merge with UserBadges
            var badgeDetails = await _badgeRepository.GetAllBadgesAsync();

            var userBadgeDetails = userBadges
                .Select(ub => new
                {
                    BadgeId = ub.BadgeId, // Ensure correct casing to match frontend model
                    Name = badgeDetails.FirstOrDefault(b => b.Id == ub.BadgeId)?.Name ?? "Unknown Badge",
                    Description = badgeDetails.FirstOrDefault(b => b.Id == ub.BadgeId)?.Description ?? "No description available"
                })
                .ToList();

            return Ok(userBadgeDetails);
        }

        // GET: api/Gamification/leaderboard
        [HttpGet("leaderboard")]
        public async Task<IActionResult> GetLeaderboard()
        {
            var topUsers = await _userGamificationRepository.GetTopUsersAsync();
            var userIds = topUsers.Select(t => t.UserId).ToList();

            var users = await _userRepository.GetUsersByIdsAsync(userIds); // ✅ Fetch all users in one query

            var leaderboard = topUsers.Select(tu => new
            {
                UserId = tu.UserId,
                UserName = users.FirstOrDefault(u => u.Id == tu.UserId)?.FullName ?? "Unknown User",
                XP = tu.XP,
                Level = tu.Level
            })
            .OrderByDescending(u => u.XP)
            .ToList();

            return Ok(leaderboard);
        }
    }
}
