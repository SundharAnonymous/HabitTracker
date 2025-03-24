using HabitTracker.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitTracker.Domain.Interfaces
{
    public interface IHabitCompletionRepository
    {
        Task<HabitCompletion> GetHabitCompletionAsync(int habitId, int userId, DateTime date);
        Task AddHabitCompletionAsync(HabitCompletion completion);
        Task UpdateHabitCompletionAsync(HabitCompletion completion);
    }

}
