


using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitGuide.Models
{
    public class WorkoutPlanExercise
    {
        [Key]
        public int WorkoutPlanId { get; set; }
        public WorkoutPlan WorkoutPlan { get; set; }
        public int ExerciseId { get; set; }
        public Exercise Exercise { get; set; }
        public int Reps { get; set; }
        public int Sets { get; set; }
        public double Weight { get; set; }
    }
}
