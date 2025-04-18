using Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class UserGoal : ModelBase
    {
        public string UserId { get; set; }
        public int GoalTemplateId { get; set; }
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public GoalTempelate? GoalTempelate { get; set; }
    }
}
