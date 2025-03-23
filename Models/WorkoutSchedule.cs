
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace FitGuide.Models
{
    public class WorkoutSchedule
    {
        public int Id { get; set; }
        public DateTime WorkoutDate { get; set; }
        public int WorkoutPlanId { get; set; }
        public WorkoutPlan WorkoutPlan { get; set; }
    }
}
