using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitTracker.Application.DTOs
{
    public class BadgeDTO
    {
        public int Id { get; set; }  // Badge ID
        public string Name { get; set; }  // Badge Name
        public string Description { get; set; }  // Badge Description
        public int XPThreshold { get; set; }  // XP required to earn this badge
    }
}
