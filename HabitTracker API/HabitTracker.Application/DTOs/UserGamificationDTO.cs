using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitTracker.Application.DTOs
{
    public class UserGamificationDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int XP { get; set; }
        public int Level { get; set; }
        public string UserName { get; set; } // ✅ Username included in DTO
    }
}
