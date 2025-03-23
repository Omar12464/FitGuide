
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;



namespace FitGuide.Models
{
    public class Meal
    {
        public int Id { get; set; }
        public string MealName { get; set; }
        public DateTime ConsumptionTime { get; set; }
        public int NutritionPlanId { get; set; }
        public NutritionPlan NutritionPlan { get; set; }
        public ICollection<MealFood> Foods { get; set; }

    }
}
