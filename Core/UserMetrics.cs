using Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class UserMetrics
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; }
        public float? BMI { get; set; }
        public float Weight { get; set; }
        public float? Fat { get; set; }
        public float? MuscleMass { get; set; }
        public float? WaterMass { get; set; }
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        //public User user { get; set; }
    }
}
