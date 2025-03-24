using HabitTracker.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitTracker.Domain.Interfaces
{
    public interface IAnalyticsRepository
    {
        Task<IEnumerable<Analytics>> GetUserAnalyticsAsync(int userId);
        Task<Analytics> GetAnalyticsByHabitAsync(int habitId);
        Task AddOrUpdateAnalyticsAsync(Analytics analytics);
    }
}
