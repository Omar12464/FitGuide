using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace FitGuide.Models
{
    public class NutritionPlan
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double CaloriesTarget { get; set; }
        public double ProteinTarget { get; set; }
        public double CarbsTarget { get; set; }
        public double FatsTarget { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int UserId { get; set; }
        public User User { get; set; }
        public ICollection<Meal> Meals { get; set; }
    }
}
