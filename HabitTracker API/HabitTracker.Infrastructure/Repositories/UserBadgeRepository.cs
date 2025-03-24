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
    public class UserBadgeRepository : IUserBadgeRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public UserBadgeRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<UserBadge>> GetUserBadgesAsync(int userId)
        {
            return await _dbContext.UserBadges
                .Where(ub => ub.UserId == userId)
                .ToListAsync();
        }

        public async Task AddUserBadgeAsync(UserBadge userBadge)
        {
            await _dbContext.UserBadges.AddAsync(userBadge);
            await _dbContext.SaveChangesAsync();
        }
    }
}
