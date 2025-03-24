using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitTracker.Domain.Entities
{
    public class Analytics
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int HabitId { get; set; }
        public string ProgressData { get; set; } // JSON or other structured format representing progress details
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
    }
}
