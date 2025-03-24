using HabitTracker.Domain.Entities;
using HabitTracker.Domain.Interfaces;
using HabitTracker.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HabitTracker.Infrastructure.Repositories
{
    public class HabitRepository : IHabitRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public HabitRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddHabitAsync(Habit habit)
        {
            await _dbContext.Habits.AddAsync(habit);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteHabitAsync(int habitId)
        {
            var habit = await _dbContext.Habits.FindAsync(habitId);
            var completions = _dbContext.HabitCompletions.Where(hc => hc.HabitId == habitId);

            if (habit != null)
            {
                _dbContext.HabitCompletions.RemoveRange(completions);
                _dbContext.Habits.Remove(habit);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<Habit> GetHabitByIdAsync(int habitId)
        {
            return await _dbContext.Habits.FindAsync(habitId);
        }

        public async Task<IEnumerable<Habit>> GetUserHabitsAsync(int userId)
        {
            return await _dbContext.Habits
                .Where(h => h.UserId == userId)
                .ToListAsync();
        }

        public async Task UpdateHabitAsync(Habit habit)
        {
            _dbContext.Habits.Update(habit);
            await _dbContext.SaveChangesAsync();
        }

        // ✅ New Method: Get habit names for multiple habit IDs
        public async Task<Dictionary<int, string>> GetHabitsByIdsAsync(IEnumerable<int> habitIds)
        {
            return await _dbContext.Habits
                .Where(h => habitIds.Contains(h.Id))
                .ToDictionaryAsync(h => h.Id, h => h.Title);
        }
    }
}
