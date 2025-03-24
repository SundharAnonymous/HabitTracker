using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitTracker.Application.DTOs
{
    public class UpdateNotificationDto
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public DateTime ScheduledTime { get; set; }
        public bool IsRead { get; set; }
    }
}
