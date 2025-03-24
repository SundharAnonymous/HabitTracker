using HabitTracker.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitTracker.Domain.Interfaces
{
    public interface IUserGamificationRepository
    {
        Task<UserGamification> GetByUserIdAsync(int userId);
        Task AddOrUpdateAsync(UserGamification userGamification);
        Task<List<UserGamification>> GetTopUsersAsync();

    }
}
