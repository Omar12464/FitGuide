using Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public enum WeightCategory
    {
        [EnumMember(Value = "Underweight")]
        Underweight, 
        [EnumMember(Value = "Normal")]
        Normal,
        [EnumMember(Value = "Overweight")]
        Overweight,
        [EnumMember(Value = "Obese")]
        Obese
    }
    public enum FitnessLevel
    {
        [EnumMember(Value = "Beginner")]
        Beginner=1,
        [EnumMember(Value = "InterMediate")]
        InterMediate=2,
        [EnumMember(Value = "Professional")]
        Professional=3
    }
    public class UserMetrics : ModelBase
    {
        public string UserId { get; set; }
        public float? BMI { get; set; }
        public float Weight { get; set; }
        public float Height { get; set; }
        public float? Fat { get; set; }
        public float? MuscleMass { get; set; }
        public float? WaterMass { get; set; }
        public WeightCategory weightCategory { get; set; }
        public FitnessLevel fitnessLevel { get; set; }
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        //public User user { get; set; }
    }
}
