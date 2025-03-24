using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitTracker.Domain.Entities
{
    public class Habit
    {
        public int Id { get; set; }
        public int UserId { get; set; } // The owner of the habit
        public string Title { get; set; }
        public string Description { get; set; }
        public string Frequency { get; set; } // e.g., "Daily", "Weekly"
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public TimeSpan? ReminderTime { get; set; } // Optional reminder time
        // Additional properties can include: StreakCount, XP, etc.
    }
}
