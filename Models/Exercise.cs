
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace FitGuide.Models
{
    public class Exercise
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string TargetMuscle { get; set; }
        public string TypeOfMachine { get; set; }
        public string Difficulty { get; set; }
        public ICollection<WorkoutPlanExercise> WorkoutPlans { get; set; }
    }
}
