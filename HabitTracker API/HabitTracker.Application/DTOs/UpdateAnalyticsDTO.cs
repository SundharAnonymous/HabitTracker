using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitTracker.Application.DTOs
{
    public class UpdateAnalyticsDTO
    {
        public int UserId { get; set; }
        public int HabitId { get; set; }
        public string ProgressData { get; set; }  // Could be a JSON string with progress info
    }
}
