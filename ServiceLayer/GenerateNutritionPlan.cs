using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer
{
    public class GenerateNutritionPlan : INutritionPlan
    {
        public double AdjustCaloriesForGoal(string goal, double tdee)
        {
            double adjustedCalories = tdee;
            switch (goal.ToLowerInvariant())
            {
                case "weight loss":
                    adjustedCalories -= 500;
                    break;
                case "muscle gain":
                    adjustedCalories += 400;

                    break;
                case "Endurance Improvement":
                    adjustedCalories += 200;

                    break;
                case "Injury Recovery (Lower Body)":
                case "Injury Recovery (Upper Body)":
                    adjustedCalories-= 200;


                    break;
                case "Post-Pregnancy Fitness":
                    adjustedCalories -= 500;

                    break;
                default:
                    adjustedCalories = tdee;

                    break;
            }
            return adjustedCalories;

        }

        public double CalculateBmr(string Gender, float weight, float height, int age)
        {
            var gender=Gender.ToLowerInvariant();

            if (gender == "male")
            {
                return 10 * weight + 6.25 * height - 5 * age + 5;
            }else if (gender == "female")
            {
                return 10 * weight + 6.25 * height - (5 * age) - 161;
            }else return 0;
        }

        public double CalculateBmr(string Gender, double weight, double height, int age)
        {
            throw new NotImplementedException();
        }

        public (double protein, double carbs, double fats) CalculateMacros(double calories, string goal)
        {
            double protein = 0;  double carbs = 0;  double fats = 0;
            switch (goal.ToLowerInvariant())
            {
                case "weight loss":
                    protein = calories * 0.3 / 4;
                    carbs = calories * 0.4 / 4;
                    fats = calories * 0.3 / 9;
                    break;
                case "muscle gain":
                    protein = calories * 0.4 / 4;
                    carbs = calories * 0.4 / 4;
                    fats = calories * 0.2 / 9;
                    break;
                case "Endurance Improvement":
                    protein = calories * 0.25 / 4;
                    carbs = calories * 0.55 / 4;
                    fats = calories * 0.2 / 9;
                    break;
                case "Injury Recovery (Lower Body)":
                case "Injury Recovery (Upper Body)":
                    protein = calories * 0.35 / 4;
                    carbs = calories * 0.45 / 4;
                    fats = calories * 0.2 / 9;
                    break;
                case "Mobility and Flexibility":
                    protein = calories * 0.25 / 4;
                    carbs = calories * 0.5 / 4;
                    fats = calories * 0.25 / 9;
                    break;
                case "Post-Pregnancy Fitness":
                    protein = calories * 0.4 / 4;
                    carbs = calories * 0.4 / 4;
                    fats = calories * 0.2 / 9;
                    break;
                default:
                    protein = calories * 0.3 / 4;
                    carbs = calories * 0.4 / 4;
                    fats = calories * 0.3 / 9;
                    break;
            }
            return (protein, carbs, fats);
        }

        //public Task<ActionResult> CreateNutritionPlanAsync(string userId, string goal)
        //{
        //   double BMR=CalculateBmr()
        //}

        public Task<ActionResult> UpdateNutritionPlanAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ValidateNutritionPlanAsync()
        {
            throw new NotImplementedException();
        }
    }
}
