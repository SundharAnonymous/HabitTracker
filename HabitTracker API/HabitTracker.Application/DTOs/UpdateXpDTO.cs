using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitTracker.Application.DTOs
{
    public class UpdateXpDto
    {
        public int UserId { get; set; }
        public int XpEarned { get; set; }
    }
}
