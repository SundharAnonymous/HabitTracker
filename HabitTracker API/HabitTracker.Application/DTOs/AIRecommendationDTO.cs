using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitTracker.Application.DTOs
{
    public class AIRecommendationDto
    {
        public int UserId { get; set; }
        public string Recommendation { get; set; }
    }
}
