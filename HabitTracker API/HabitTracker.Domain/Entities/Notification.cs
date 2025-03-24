using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitTracker.Domain.Entities
{
    public class Notification
    {
        public int Id { get; set; }
        public int UserId { get; set; }           // The user to receive the notification
        public string Message { get; set; }         // Notification message text
        public DateTime ScheduledTime { get; set; } // When the notification should be sent
        public bool IsRead { get; set; }            // Indicates if the notification has been seen
    }
}
