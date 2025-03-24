using HabitTracker.Domain.Entities;

public interface IHabitRepository
{
    Task<Habit> GetHabitByIdAsync(int habitId);
    Task<IEnumerable<Habit>> GetUserHabitsAsync(int userId);
    Task AddHabitAsync(Habit habit);
    Task UpdateHabitAsync(Habit habit);
    Task DeleteHabitAsync(int habitId);

    // ✅ New Method: Get habit names for multiple habit IDs
    Task<Dictionary<int, string>> GetHabitsByIdsAsync(IEnumerable<int> habitIds);
}
