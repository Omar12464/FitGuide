
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace FitGuide.Models
{
    public class MealFood
    {
        [Key]
        public int MealId { get; set; }
        public Meal Meal { get; set; }
        public int FoodId { get; set; }
        public Food Food { get; set; }
        public double ServingSize { get; set; }
    }
}
