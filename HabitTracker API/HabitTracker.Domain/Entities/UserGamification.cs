using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitTracker.Domain.Entities
{
    public class UserGamification
    {
        public int Id { get; set; }
        public int UserId { get; set; }  // Reference to the user
        public int XP { get; set; }      // Experience points earned
        public int Level { get; set; }   // User level, derived from XP
    }
}
