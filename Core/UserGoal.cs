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
        //public int GoalTemplateId { get; set; }
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public string? name { get; set; }
        public float? targetBMI { get; set; }
        public float? targetWeight { get; set; }
        public float? targetMuscleMass { get; set; }
        public float? targetWaterMass { get; set; }
        public string? description { get; set; }
    }
}
