
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;




namespace FitGuide.Models
{
    public class UserMetrics
    {
        public int Id { get; set; }
        public double BMI { get; set; }
        public double BodyFat { get; set; }
        public double MuscleMass { get; set; }
        public double WaterMass { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
