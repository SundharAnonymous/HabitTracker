using HabitTracker.Domain.Entities;
using HabitTracker.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HabitTracker.Infrastructure.Services
{
    public class GamificationService : IGamificationService
    {
        private readonly IUserGamificationRepository _userGamificationRepository;
        private readonly IBadgeRepository _badgeRepository;
        private readonly IUserBadgeRepository _userBadgeRepository;

        public GamificationService(
            IUserGamificationRepository userGamificationRepository,
            IBadgeRepository badgeRepository,
            IUserBadgeRepository userBadgeRepository)
        {
            _userGamificationRepository = userGamificationRepository;
            _badgeRepository = badgeRepository;
            _userBadgeRepository = userBadgeRepository;
        }

        // ✅ Increase XP, update level, and check for badges
        public async Task<UserGamification> RewardXP(int userId, int xpEarned)
        {
            try
            {
                var gamification = await _userGamificationRepository.GetByUserIdAsync(userId);
                if (gamification == null)
                {
                    gamification = new UserGamification
                    {
                        UserId = userId,
                        XP = 0,
                        Level = 1
                    };
                }

                gamification.XP += xpEarned;
                gamification.Level = (gamification.XP / 100) + 1; // Level up every 100 XP

                await _userGamificationRepository.AddOrUpdateAsync(gamification);
                await CheckAndAwardBadge(userId, gamification.XP);

                return gamification;
            }
            catch(Exception ex){
                return null;
            }
        }

        // ✅ Award badges based on XP milestones
        public async Task<UserBadge?> CheckAndAwardBadge(int userId, int totalXp)
        {
            var allBadges = await _badgeRepository.GetAllBadgesAsync();
            var earnedBadges = await _userBadgeRepository.GetUserBadgesAsync(userId);
            var earnedBadgeIds = earnedBadges.Select(b => b.BadgeId).ToHashSet();

            foreach (var badge in allBadges)
            {
                if (totalXp >= badge.XPThreshold && !earnedBadgeIds.Contains(badge.Id))
                {
                    var userBadge = new UserBadge { UserId = userId, BadgeId = badge.Id };
                    await _userBadgeRepository.AddUserBadgeAsync(userBadge);
                    return userBadge;
                }
            }
            return null;
        }
    }
}
