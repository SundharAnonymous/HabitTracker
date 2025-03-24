using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitTracker.Application.DTOs
{
    public class MarkHabitDTO
    {
        public int HabitId { get; set; }
        public int UserId { get; set; }
        public bool IsCompleted { get; set; }
    }

}
