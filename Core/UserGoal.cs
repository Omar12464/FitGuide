using Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class UserGoal
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; }
        public string TypeName { get; set; }
        public float? BMI { get; set; }
        public float? weights { get; set; }
        public float? MuscleMass { get; set; }
        public float? WaterMass { get; set; }
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    }
}
