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
    public class HabitCompletionRepository : IHabitCompletionRepository
    {
        private readonly ApplicationDbContext _context;

        public HabitCompletionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<HabitCompletion> GetHabitCompletionAsync(int habitId, int userId, DateTime date)
        {
            // Use EF Core's FirstOrDefaultAsync to fetch the record.
            return await _context.HabitCompletions
                .FirstOrDefaultAsync(x => x.HabitId == habitId
                                        && x.UserId == userId
                                        && x.CompletedDate == date);
        }

        public async Task AddHabitCompletionAsync(HabitCompletion completion)
        {
            _context.HabitCompletions.Add(completion);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateHabitCompletionAsync(HabitCompletion completion)
        {
            _context.HabitCompletions.Update(completion);
            await _context.SaveChangesAsync();
        }
    }
}
