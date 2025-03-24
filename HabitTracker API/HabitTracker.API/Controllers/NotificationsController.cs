using HabitTracker.Application.DTOs;
using HabitTracker.Domain.Entities;
using HabitTracker.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HabitTracker.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationRepository _notificationRepository;

        public NotificationsController(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        // ✅ GET: api/Notifications/user
        [HttpGet("user")]
        public async Task<IActionResult> GetUserNotifications()
        {
            if (!TryGetUserId(out int userId))
                return Unauthorized("User ID not found in token.");

            var notifications = await _notificationRepository.GetUserNotificationsAsync(userId);
            return Ok(notifications);
        }

        // ✅ GET: api/Notifications/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetNotification(int id)
        {
            if (!TryGetUserId(out int userId))
                return Unauthorized("User ID not found in token.");

            var notification = await _notificationRepository.GetNotificationByIdAsync(id);
            if (notification == null || notification.UserId != userId)
                return NotFound("Notification not found or unauthorized access.");

            return Ok(notification);
        }

        // ✅ POST: api/Notifications/create
        [HttpPost("create")]
        public async Task<IActionResult> CreateNotification([FromBody] CreateNotificationDTO createDto)
        {
            if (createDto == null)
                return BadRequest();

            if (!TryGetUserId(out int userId))
                return Unauthorized("User ID not found in token.");

            var notification = new Notification
            {
                UserId = userId,  // ✅ Fetch userId from token
                Message = createDto.Message,
                ScheduledTime = createDto.ScheduledTime,
                IsRead = false
            };

            await _notificationRepository.AddNotificationAsync(notification);
            return Ok(notification);
        }

        // ✅ PUT: api/Notifications/update
        [HttpPut("update")]
        public async Task<IActionResult> UpdateNotification([FromBody] UpdateNotificationDto updateDto)
        {
            if (updateDto == null)
                return BadRequest();

            if (!TryGetUserId(out int userId))
                return Unauthorized("User ID not found in token.");

            var notification = await _notificationRepository.GetNotificationByIdAsync(updateDto.Id);
            if (notification == null || notification.UserId != userId)
                return NotFound("Notification not found or unauthorized access.");

            notification.Message = updateDto.Message;
            notification.ScheduledTime = updateDto.ScheduledTime;
            notification.IsRead = updateDto.IsRead;

            await _notificationRepository.UpdateNotificationAsync(notification);
            return Ok(notification);
        }

        // ✅ DELETE: api/Notifications/delete/{id}
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteNotification(int id)
        {
            if (!TryGetUserId(out int userId))
                return Unauthorized("User ID not found in token.");

            var notification = await _notificationRepository.GetNotificationByIdAsync(id);
            if (notification == null || notification.UserId != userId)
                return NotFound("Notification not found or unauthorized access.");

            await _notificationRepository.DeleteNotificationAsync(id);
            return Ok();
        }

        // ✅ Private helper method to extract user ID from token
        private bool TryGetUserId(out int userId)
        {
            userId = 0;
            var userIdClaim = HttpContext.User.Claims.FirstOrDefault(c =>
                c.Type == ClaimTypes.NameIdentifier || c.Type == "sub");

            if (userIdClaim == null) return false;
            return int.TryParse(userIdClaim.Value, out userId);
        }
    }
}
