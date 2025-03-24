using HabitTracker.Domain.Entities;
using HabitTracker.Domain.Interfaces;
using HabitTracker.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitTracker.Infrastructure.Repositories
{
    public class UserGamificationRepository : IUserGamificationRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public UserGamificationRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<UserGamification> GetByUserIdAsync(int userId)
        {
            return await _dbContext.UserGamifications
                .FirstOrDefaultAsync(ug => ug.UserId == userId);
        }


        public async Task AddOrUpdateAsync(UserGamification userGamification)
        {
            var existing = await GetByUserIdAsync(userGamification.UserId);
            if (existing == null)
            {
                await _dbContext.UserGamifications.AddAsync(userGamification);
            }
            else
            {
                existing.XP = userGamification.XP;
                existing.Level = userGamification.Level;
                _dbContext.UserGamifications.Update(existing);
            }
            await _dbContext.SaveChangesAsync();
        }
        public async Task<List<UserGamification>> GetTopUsersAsync()
        {
            return await _dbContext.UserGamifications
                .OrderByDescending(u => u.XP)
                .Take(10) // Get top 10 users
                .ToListAsync();
        }

    }
}
