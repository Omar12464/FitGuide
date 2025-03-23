using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitGuide.Models
{
 

public class User
    {
        public int Id { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public DateTime BirthDate { get; set; }
        public enum GenderType { Male, Female }
        public GenderType Gender { get; set; }

        public int Age { get; set; }
        
        public ICollection<UserMetrics> Metrics { get; set; }
        public ICollection<UserGoal> Goals { get; set; }
        public ICollection<WorkoutPlan> WorkoutPlans { get; set; }
        public ICollection<NutritionPlan> NutritionPlans { get; set; }
    }

 
   
}

