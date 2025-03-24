using HabitTracker.Domain.Entities;
using System.Threading.Tasks;

namespace HabitTracker.Domain.Interfaces
{
    public interface IGamificationService
    {
        /// <summary>
        /// Increases XP for a user and checks for level-ups and badge rewards.
        /// </summary>
        Task<UserGamification> RewardXP(int userId, int xpEarned);

        /// <summary>
        /// Checks if a user qualifies for a new badge based on XP and awards it.
        /// </summary>
        Task<UserBadge?> CheckAndAwardBadge(int userId, int totalXp);
    }
}
