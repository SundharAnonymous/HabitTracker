using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitTracker.Domain.Entities
{
    public class HabitCompletion
    {
        public int Id { get; set; }
        public int HabitId { get; set; }
        public int UserId { get; set; }
        public DateTime CompletedDate { get; set; }
        public bool IsCompleted { get; set; }
    }
}
