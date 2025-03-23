
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;



namespace FitGuide.Models
{
    public class UserGoal
    {

        public int Id { get; set; }
        public double Weight { get; set; }
        public double BMI { get; set; }
        public double MuscleMass { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsAchieved { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
