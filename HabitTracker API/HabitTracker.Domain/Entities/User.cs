using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitTracker.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }  // Unique identifier for login
        public string PasswordHash { get; set; }  // Hashed password
        public string FullName { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}
