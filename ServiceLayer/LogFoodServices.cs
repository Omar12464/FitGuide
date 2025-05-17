using Core;
using Microsoft.EntityFrameworkCore;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer
{
    public class LogFoodServices:ILogFoodService
    {
        private readonly FitGuideContext _fitGuideContext;

        public LogFoodServices(FitGuideContext fitGuideContext)
        {
            _fitGuideContext = fitGuideContext;
        }
        public async Task LogFood(string userId, int foodItemId, double quantity)
        {
            var food = await _fitGuideContext.Food.FindAsync(foodItemId);
            if (food == null)
            {
                throw new Exception("Food item not found");
            }
            var userllergies = await _fitGuideContext.userAllergies
                .Where(x => x.UserId == userId)
                .Select(x => x.allergy)
                .ToListAsync();

            if (!IsFoodSafeForUser(food, userllergies.Select(x => x.Name).ToList()))
            {
                throw new Exception("Food item is not safe for the user due to allergies");
            }
            var log = new LogFood
            {
                UserId = userId,
                FoodId = foodItemId,
                Quantity = quantity,
                LoggedAt = DateTime.UtcNow
            };
            var calories = (food.CaloriesPerServing / 100) * quantity;
            var protein = (food.ProteinPerServing / 100) * quantity;
            var carbs = (food.CarbsPerServing / 100) * quantity;
            var fat = (food.FatPerServing / 100) * quantity;

            var today = DateTime.UtcNow.Date;
            var dailyIntake = await _fitGuideContext.dailyIntakes
                .Where(l => l.UserId == userId && l.Date == today)
                .FirstOrDefaultAsync();

            if (dailyIntake == null)
            {
                dailyIntake = new DailyIntake
                {
                    UserId = userId,
                    Date = today,
                    TotalCalories = calories,
                    TotalProtein = protein,
                    TotalCarbs = carbs,
                    TotalFat = fat

                };
                _fitGuideContext.dailyIntakes.Add(dailyIntake);
                await _fitGuideContext.SaveChangesAsync();

            }
            else
            {
                dailyIntake.TotalCalories += calories;
                dailyIntake.TotalProtein += protein;
                dailyIntake.TotalCarbs += carbs;
                dailyIntake.TotalFat += fat;
                _fitGuideContext.dailyIntakes.Update(dailyIntake);
                await _fitGuideContext.SaveChangesAsync();
            }
            _fitGuideContext.LogFood.Add(log);
            await _fitGuideContext.SaveChangesAsync();
        }

        public bool IsFoodSafeForUser(FoodItem food, List<string> userAllergies)
        {
            foreach (var allergy in userAllergies)
            {
                switch (allergy)
                {
                    case "Peanut Allergy":
                        if (!food.IsPeanutFree) return false;
                        break;
                    case "Tree Nut Allergy":
                        if (!food.IsNutFree) return false;
                        break;
                    case "Milk Allergy":
                        if (!food.IsDairyFree) return false;
                        break;
                    case "Shellfish Allergy":
                        if (!food.IsShellfishFree) return false;
                        break;
                    case "Fish Allergy":
                        if (!food.IsFishFree) return false;
                        break;
                    case "Soy Allergy":
                        if (!food.IsSoyFree) return false;
                        break;
                    case "Wheat Allergy":
                        if (!food.IsWheatFree) return false;
                        break;
                    case "Sesame Allergy":
                        if (!food.IsSesameFree) return false;
                        break;
                    case "Mustard Allergy":
                        if (!food.IsMustardFree) return false;
                        break;
                    case "Celery Allergy":
                        if (!food.IsCeleryFree) return false;
                        break;
                    case "Lupin Allergy":
                        if (!food.IsLupinFree) return false;
                        break;
                    case "Molluscs Allergy":
                        if (!food.IsMolluscsFree) return false;
                        break;
                    case "Sulfite Sensitivity":
                        if (!food.IsSulfiteFree) return false;
                        break;
                    case "Gluten Intolerance (Celiac Disease)":
                        if (!food.IsGlutenFree) return false;
                        break;
                    case "Fruit Allergy (e.g., Kiwi, Avocado)":
                        if (!food.IsFruitFree) return false;
                        break;
                    case "Legume Allergy (e.g., Lentils, Chickpeas)":
                        if (!food.IsLegumeFree) return false;
                        break;
                }
            }
            return true;
        }

    }
}

