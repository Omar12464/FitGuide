using Core.Identity.Entities;
using Core.Interface;
using Core;
using FitGuide.DTOs;
using FitGuide.ErrorsManaged;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Repository;
using ServiceLayer;
using Microsoft.EntityFrameworkCore;

namespace FitGuide.Controllers
{

    public class NutritionPlanController : BaseAPI
    {
        private readonly FitGuideContext _fitGuideContext;
        private readonly IGeneric<NutritionPlan> _repoNutrition;
        private readonly INutritionPlan _nutritionPlanServices;
        private readonly UserManager<User> _userManager;
        private readonly IGeneric<UserMetrics> _repoMetrics;
        public NutritionPlanController(FitGuideContext fitGuideContext, IGeneric<NutritionPlan> repoNutrition, INutritionPlan nutritionPlanServices, UserManager<User> userManager, IGeneric<UserMetrics> repoMetrics)
        {
            _fitGuideContext = fitGuideContext;
            _repoNutrition = repoNutrition;
            _nutritionPlanServices = nutritionPlanServices;
            _userManager = userManager;
            _repoMetrics = repoMetrics;
        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("GenerateNutritionPlan")]
        public async Task<ActionResult> GenerateNutriotionPlan()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return BadRequest(new ApiValidationErrorResponse { Errors = new[] { "User UnAuthorized" } });
            }
            var nutritionPlan = await _fitGuideContext.nutritionPlans.Where(np => user.Id == np.UserId).ToListAsync();
            if (nutritionPlan.Any())
            {
                return BadRequest(new ApiValidationErrorResponse { Errors = new[] { "User has already have a nutrition plan" } });
            }
            var userMetrics = await _repoMetrics.GetAllAsync();
            var userMetric = userMetrics.OrderByDescending(um => um.CreatedAt).FirstOrDefault(um => um.UserId == user.Id);
            if (userMetric == null)
            {
                return BadRequest(new ApiValidationErrorResponse { Errors = new[] {"User has no metrics" } });

            }
            var bmr = _nutritionPlanServices.CalculateBmr(user.Gender, userMetric.Weight, userMetric.Height, user.Age);
            double tdee = 0;
            var gymFreq = userMetric.GymFrequency;
            if (gymFreq == GymFrequency.Everyday)
            {
                tdee = bmr * 1.725;
            }
            else if (gymFreq == GymFrequency.ThreeToFour)
            {
                tdee = bmr * 1.375;
            }
            else if (gymFreq == GymFrequency.FiveToSix)
            {
                tdee = bmr * 1.55;
            }
            else if (gymFreq == GymFrequency.OneToTwo)
            {
                tdee = bmr * 1.2;
            }
            else
            {
                tdee = bmr * 1.2;
            }
            var userGoal = await _fitGuideContext.userGoals.OrderByDescending(ug => ug.CreatedAt).FirstOrDefaultAsync(ug => ug.UserId == user.Id && ug.IsActive);
            if (userGoal == null)
            {
                return BadRequest(new ApiValidationErrorResponse { Errors = new[] { "User has no active goals" } });
            }
            var TotalDailyCalories = _nutritionPlanServices.AdjustCaloriesForGoal(userGoal.name, tdee);
            var Macros = _nutritionPlanServices.CalculateMacros(TotalDailyCalories, userGoal.name);
            var nutritionPlanEntity = new NutritionPlan
            {
                Name = $" Nutrition Plan has been generated for {user.FistName}",
                UserId = user.Id,
                CaloriestTarget = Macros.calories,
                ProteinTarget = Macros.protein,
                CarbsTarget = Macros.carbs,
                FatTarget = Macros.fats,
                CreatedAt = DateTime.UtcNow,
            };

               await _repoNutrition.AddAsync(nutritionPlanEntity);
                return Ok(new { Message = "Nutrition plan generated successfully." });
            

        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut("UpdateNutritionPlan")]
        public async Task<ActionResult> UpdateNutritionPlan()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return BadRequest(new ApiValidationErrorResponse { Errors = new[] { "User Unauthorized" } });
            }

            // Fetch the latest metrics and goals
            var userMetrics = await _repoMetrics.GetAllAsync();
            var userMetric = userMetrics
                .OrderByDescending(um => um.CreatedAt)
                .FirstOrDefault(um => um.UserId == user.Id);

            var userGoal = await _fitGuideContext.userGoals
                .OrderByDescending(ug => ug.CreatedAt)
                .FirstOrDefaultAsync(ug => ug.UserId == user.Id && ug.IsActive);

            if (userMetric == null || userGoal == null)
            {
                return BadRequest(new ApiValidationErrorResponse { Errors = new[] { "Insufficient data to update nutrition plan" } });
            }

            // Calculate BMR, TDEE, and total daily calories
            var bmr = _nutritionPlanServices.CalculateBmr(user.Gender, userMetric.Weight, userMetric.Height, user.Age);
            var gymFrequency = userMetric.GymFrequency;
            double tdee = 0;
            var gymFreq =userMetric.GymFrequency;
            if (gymFreq == GymFrequency.Everyday)
            {
                tdee = bmr * 1.725;
            }
            else if (gymFreq == GymFrequency.ThreeToFour)
            {
                tdee = bmr * 1.375;
            }
            else if (gymFreq == GymFrequency.FiveToSix)
            {
                tdee = bmr * 1.55;
            }
            else if (gymFreq == GymFrequency.OneToTwo)
            {
                tdee = bmr * 1.2;
            }
            else
            {
                tdee = bmr * 1.2;
            }
            var totalDailyCalories = _nutritionPlanServices.AdjustCaloriesForGoal(userGoal.name, tdee);

            // Calculate macros
            var macros = _nutritionPlanServices.CalculateMacros(totalDailyCalories, userGoal.name);

            // Update the existing nutrition plan
            var existingNutritionPlan = await _fitGuideContext.nutritionPlans
                .FirstOrDefaultAsync(np => np.UserId == user.Id);

            if (existingNutritionPlan != null)
            {
                existingNutritionPlan.CaloriestTarget = macros.calories;
                existingNutritionPlan.ProteinTarget = macros.protein;
                existingNutritionPlan.CarbsTarget = macros.carbs;
                existingNutritionPlan.FatTarget = macros.fats;
                existingNutritionPlan.CreatedAt = DateTime.UtcNow;

                _fitGuideContext.Update(existingNutritionPlan);
                await _fitGuideContext.SaveChangesAsync();

                return Ok(new { Message = "Nutrition plan updated successfully." });
            }

            return BadRequest(new ApiValidationErrorResponse { Errors = new[] { "No existing nutrition plan found" } });
        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("GetMyNutritionPlan")]
        public async Task<ActionResult> GetMyNutritionPlan()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return BadRequest(new ApiValidationErrorResponse { Errors = new[] { "User Unauthorized" } });
            }
            var nutritionPlan = await _fitGuideContext.nutritionPlans.OrderByDescending(np => np.CreatedAt)
                .FirstOrDefaultAsync(np => np.UserId == user.Id);
            if (nutritionPlan == null)
            {
                return BadRequest(new ApiValidationErrorResponse { Errors = new[] { "No nutrition plan found" } });
            }
            return Ok(new
            {
                Name = nutritionPlan.Name,
                TotalCalories = nutritionPlan.CaloriestTarget,
                ProteinTarget = nutritionPlan.ProteinTarget,
                CarbsTarget = nutritionPlan.CarbsTarget,
                FatTarget = nutritionPlan.FatTarget,
                CreatedAt = nutritionPlan.CreatedAt
            });

        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpDelete("DeleteNutritionPlan")]
        public async Task<ActionResult> DeleteNutritionPlan()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return BadRequest(new ApiValidationErrorResponse { Errors = new[] { "User Unauthorized" } });
            }
            var nutritionPlan = await _fitGuideContext.nutritionPlans
                .FirstOrDefaultAsync(np => np.UserId == user.Id);
            if (nutritionPlan == null)
            {
                return BadRequest(new ApiValidationErrorResponse { Errors = new[] { "No nutrition plan found" } });
            }
            _repoNutrition.DeleteAsync(nutritionPlan);
            return Ok(new { Message = "Nutrition plan deleted successfully." });
        }
    }
}
