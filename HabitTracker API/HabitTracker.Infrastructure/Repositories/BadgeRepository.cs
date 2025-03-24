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
    public class BadgeRepository : IBadgeRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public BadgeRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Badge>> GetAllBadgesAsync()
        {
            return await _dbContext.Badges.ToListAsync();
        }

        public async Task<Badge> GetBadgeByIdAsync(int badgeId)
        {
            return await _dbContext.Badges.FindAsync(badgeId);
        }
    }
}
