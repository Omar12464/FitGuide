using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer
{
    public interface INutritionPlan
    {
        //public Task<ActionResult> CreateNutritionPlanAsync(string userId, string goal);

        public Task<ActionResult> UpdateNutritionPlanAsync(string userId);

        public Task<bool> ValidateNutritionPlanAsync();
        public double CalculateBmr(string Gender, double weight, double height,int age);
        public double AdjustCaloriesForGoal(string goal, double tdee);
        public (double protein, double carbs, double fats) CalculateMacros(double calories, string goal);
        //public double CalculateProtein(float weight);
        //public double CalculateFats(float CaloriesPerDay, float ProteinInGrams);
        //public double CalculateCarbs(float CaloriesPerDay,float ProteinInGrams,float FatsInGrams);
    }
}
