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
    public class AnalyticsRepository : IAnalyticsRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public AnalyticsRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Analytics>> GetUserAnalyticsAsync(int userId)
        {
            return await _dbContext.Analytics
                .Where(a => a.UserId == userId)
                .ToListAsync();
        }


        public async Task<Analytics> GetAnalyticsByHabitAsync(int habitId)
        {
            return await _dbContext.Analytics
                    .Where(a => a.HabitId == habitId)
                    .FirstOrDefaultAsync();
        }

        public async Task AddOrUpdateAnalyticsAsync(Analytics analytics)
        {
        try{
                var existing = await _dbContext.Analytics
                       .FirstOrDefaultAsync(a => a.HabitId == analytics.HabitId && a.UserId == analytics.UserId);


                if (existing == null)
                {
                    await _dbContext.Analytics.AddAsync(analytics);
                }
                else
                {
                    // ✅ Update existing progress data and timestamp
                    existing.ProgressData = analytics.ProgressData;
                    existing.LastUpdated = analytics.LastUpdated;
                    _dbContext.Analytics.Update(existing);
                }
                await _dbContext.SaveChangesAsync();

            }
            catch(Exception ex){
                Console.Write(ex);
            }
           
        }
    }
}
