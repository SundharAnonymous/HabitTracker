using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitTracker.Domain.Entities
{
    public class Badge
    {
        public int Id { get; set; }  // Primary Key
        public string Name { get; set; }  // Badge Name
        public string Description { get; set; }  // Badge Description
        public int XPThreshold { get; set; }  // XP required for this badge
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;  // Timestamp
    }
}
