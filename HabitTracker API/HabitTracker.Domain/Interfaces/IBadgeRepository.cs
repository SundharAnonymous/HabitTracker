using HabitTracker.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitTracker.Domain.Interfaces
{
    public interface IBadgeRepository
    {
        Task<IEnumerable<Badge>> GetAllBadgesAsync();
        Task<Badge> GetBadgeByIdAsync(int badgeId);
    }
}
