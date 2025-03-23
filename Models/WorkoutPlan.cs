
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;



namespace FitGuide.Models
{
    public class WorkoutPlan
    {
        
        public int Id { get; set; }
        public string Name { get; set; }
        public int NoDaysPerWeek { get; set; }
        public string Description { get; set; }
        public string GoalType { get; set; }
        public ICollection<WorkoutSchedule> Schedules { get; set; }
        public ICollection<WorkoutPlanExercise> Exercises { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
