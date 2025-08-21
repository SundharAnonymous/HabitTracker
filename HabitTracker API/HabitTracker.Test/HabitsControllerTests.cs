using FakeItEasy;
using HabitTracker.API.Controllers;
using HabitTracker.Domain.Entities;
using HabitTracker.Domain.Interfaces;
using HabitTracker.Infrastructure.Repositories;
using Xunit;

namespace HabitTracker.Test
{
    public class HabitsControllerTests
    {
        private readonly IHabitRepository _fakeHabitRepo;
        private readonly IHabitCompletionRepository _fakeHabitCompletionRepo;
        private readonly IAnalyticsRepository _fakeAnalyticsRepo;
        private readonly IGamificationService _fakeGamificationService;
        private readonly HabitsController _habitController;

        public HabitsControllerTests()
        {
            _fakeHabitRepo = A.Fake<IHabitRepository>();
            _fakeHabitCompletionRepo = A.Fake<IHabitCompletionRepository>();
            _fakeAnalyticsRepo = A.Fake<IAnalyticsRepository>();
            _fakeGamificationService = A.Fake<IGamificationService>();
        }

        [Fact]
        public void MarkHabitCompletion_ShouldMarkHabitAsCompleted()
        {
            A.CallTo(() => _fakeGamificationService.RewardXP(1, 100))
                .Returns(Task.FromResult(new UserGamification { UserId = 1, XP = 100 }));
        }

    }
}
