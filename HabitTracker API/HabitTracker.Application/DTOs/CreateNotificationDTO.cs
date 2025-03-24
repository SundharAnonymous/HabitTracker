using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitTracker.Application.DTOs
{
    public class CreateNotificationDTO
    {
        public string Message { get; set; }
        public DateTime ScheduledTime { get; set; }
    }
}
