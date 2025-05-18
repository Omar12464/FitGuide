using Core;
using Core.Identity.Entities;
using Core.Interface;
using FitGuide.DTOs;
using FitGuide.ErrorsManaged;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.HttpSys;
using Microsoft.EntityFrameworkCore;
using Repository;
using ServiceLayer;

namespace FitGuide.Controllers
{

    public class HomeContoller : BaseAPI
    {
        private readonly FitGuideContext _fitGuideContext;
        private readonly ILogFoodService _logFoodService;
        private readonly IGeneric<DailyIntake> _genericDailyIntake;
        private readonly IGeneric<NutritionPlan> _repoNutrition;
        private readonly INutritionPlan _nutritionPlanServices;
        private readonly UserManager<User> _userManager;
        private readonly IGeneric<UserMetrics> _repoMetrics;

        public HomeContoller(FitGuideContext fitGuideContext,ILogFoodService logFoodService,IGeneric<DailyIntake> genericDailyIntake, IGeneric<NutritionPlan> repoNutrition, INutritionPlan nutritionPlanServices, UserManager<User> userManager, IGeneric<UserMetrics> repoMetrics)
        {
            _fitGuideContext = fitGuideContext;
            _logFoodService = logFoodService;
            _genericDailyIntake = genericDailyIntake;
            _repoNutrition = repoNutrition;
            _nutritionPlanServices = nutritionPlanServices;
            _userManager = userManager;
            _repoMetrics = repoMetrics;
        }
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        //[HttpPost("GenerateNutritionPlan")]
        //public async Task<ActionResult> GenerateNutriotionPlan(NutritionPlanInputDTO nutritionPlanInputDTO)
        //{
        //    var user = await _userManager.GetUserAsync(User);
        //    if (user == null)
        //    {
        //        return BadRequest(new ApiValidationErrorResponse { Errors = new[] { "User UnAuthorized" } });
        //    }
        //    var nutritionPlan = await _fitGuideContext.nutritionPlans.Where(np => user.Id == np.UserId).ToListAsync();
        //    if (nutritionPlan.Any())
        //    {
        //        return BadRequest(new ApiValidationErrorResponse { Errors = new[] { "User has already have a nutrition plan" } });
        //    }
        //    var userMetrics = await _repoMetrics.GetAllAsync();
        //    var userMetric = userMetrics.OrderByDescending(um => um.CreatedAt).FirstOrDefault(um => um.UserId == user.Id);
        //    var bmr = _nutritionPlanServices.CalculateBmr(user.Gender, userMetric.Weight, userMetric.Height, user.Age);
        //    var tdee = bmr * userMetric.GymFrequency;
        //    var userGoal = await _fitGuideContext.userGoals.OrderByDescending(ug => ug.CreatedAt).FirstOrDefaultAsync(ug => ug.UserId == user.Id && ug.IsActive);
        //    var TotalDailyCalories = _nutritionPlanServices.AdjustCaloriesForGoal(userGoal.name, tdee);
        //    var Macros = _nutritionPlanServices.CalculateMacros(TotalDailyCalories, userGoal.name);
        //    var nutritionPlanEntity = new NutritionPlan
        //    {
        //        Name = $" Nutrition Plan has been generated for {user.FistName}",
        //        UserId = user.Id,
        //        ProteinTarget = Macros.protein,
        //        CarbsTarget = Macros.carbs,
        //        FatTarget = Macros.fats,
        //        CreatedAt = DateTime.UtcNow,
        //    };
        //    try
        //    {
        //        _repoNutrition.AddAsync(nutritionPlanEntity);
        //        return Ok(new { Message = "Nutrition plan generated successfully." });
        //    }
        //    catch (Exception ex)
        //    {

        //        return (BadRequest(new ApiValidationErrorResponse { Errors = new[] { ex.Message } }));
        //    }
        //}
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        //[HttpPut("UpdateNutritionPlan")]
        //public async Task<ActionResult> UpdateNutritionPlan()
        //{
        //    var user = await _userManager.GetUserAsync(User);
        //    if (user == null)
        //    {
        //        return BadRequest(new ApiValidationErrorResponse { Errors = new[] { "User Unauthorized" } });
        //    }

        //    // Fetch the latest metrics and goals
        //    var userMetrics = await _repoMetrics.GetAllAsync();
        //    var userMetric = userMetrics
        //        .OrderByDescending(um => um.CreatedAt)
        //        .FirstOrDefault(um => um.UserId == user.Id);

        //    var userGoal = await _fitGuideContext.userGoals
        //        .OrderByDescending(ug => ug.CreatedAt)
        //        .FirstOrDefaultAsync(ug => ug.UserId == user.Id && ug.IsActive);

        //    if (userMetric == null || userGoal == null)
        //    {
        //        return BadRequest(new ApiValidationErrorResponse { Errors = new[] { "Insufficient data to update nutrition plan" } });
        //    }

        //    // Calculate BMR, TDEE, and total daily calories
        //    var bmr = _nutritionPlanServices.CalculateBmr(user.Gender, userMetric.Weight, userMetric.Height, user.Age);
        //    var tdee = bmr * userMetric.GymFrequency;
        //    var totalDailyCalories = _nutritionPlanServices.AdjustCaloriesForGoal(userGoal.name, tdee);

        //    // Calculate macros
        //    var macros = _nutritionPlanServices.CalculateMacros(totalDailyCalories, userGoal.name);

        //    // Update the existing nutrition plan
        //    var existingNutritionPlan = await _fitGuideContext.nutritionPlans
        //        .FirstOrDefaultAsync(np => np.UserId == user.Id);

        //    if (existingNutritionPlan != null)
        //    {
        //        existingNutritionPlan.ProteinTarget = macros.protein;
        //        existingNutritionPlan.CarbsTarget = macros.carbs;
        //        existingNutritionPlan.FatTarget = macros.fats;
        //        existingNutritionPlan.CreatedAt = DateTime.UtcNow;

        //        _fitGuideContext.Update(existingNutritionPlan);
        //        await _fitGuideContext.SaveChangesAsync();

        //        return Ok(new { Message = "Nutrition plan updated successfully." });
        //    }

        //    return BadRequest(new ApiValidationErrorResponse { Errors = new[] { "No existing nutrition plan found" } });
        //}
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        //[HttpGet("GetMyNutritionPlan")]
        //public async Task<ActionResult> GetMyNutritionPlan()
        //{
        //    var user = await _userManager.GetUserAsync(User);
        //    if (user == null)
        //    {
        //        return BadRequest(new ApiValidationErrorResponse { Errors = new[] { "User Unauthorized" } });
        //    }
        //    var nutritionPlan = await _fitGuideContext.nutritionPlans.OrderByDescending(np => np.CreatedAt)
        //        .FirstOrDefaultAsync(np => np.UserId == user.Id);
        //    if (nutritionPlan == null)
        //    {
        //        return BadRequest(new ApiValidationErrorResponse { Errors = new[] { "No nutrition plan found" } });
        //    }
        //    return Ok(new
        //    {
        //        Name = nutritionPlan.Name,
        //        ProteinTarget = nutritionPlan.ProteinTarget,
        //        CarbsTarget = nutritionPlan.CarbsTarget,
        //        FatTarget = nutritionPlan.FatTarget,
        //        CreatedAt = nutritionPlan.CreatedAt
        //    });

        //}
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        //[HttpDelete("DeleteNutritionPlan")]
        //public async Task<ActionResult> DeleteNutritionPlan()
        //{
        //    var user = await _userManager.GetUserAsync(User);
        //    if (user == null)
        //    {
        //        return BadRequest(new ApiValidationErrorResponse { Errors = new[] { "User Unauthorized" } });
        //    }
        //    var nutritionPlan = await _fitGuideContext.nutritionPlans
        //        .FirstOrDefaultAsync(np => np.UserId == user.Id);
        //    if (nutritionPlan == null)
        //    {
        //        return BadRequest(new ApiValidationErrorResponse { Errors = new[] { "No nutrition plan found" } });
        //    }
        //    _repoNutrition.DeleteAsync(nutritionPlan);
        //    return Ok(new { Message = "Nutrition plan deleted successfully." });
        //}
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("GetFirstFiveWorkoutExercises")]
        public async Task<ActionResult> GetFirstFiveWorkouts()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return BadRequest(new ApiValidationErrorResponse { Errors = new[] { "User Unauthorized" } });
            }
            var workouts = _fitGuideContext.workOutExercises
                .Where(we => we.UserId == user.Id)
                .Include(we => we.workOutPlan)
                .Include(we => we.exercise)
                .Take(5)
                .ToListAsync();
            var workoutList = await workouts;
            if (!workoutList.Any())
            {
                return NotFound(new { message = "No workout plans found for the user." });
            }
            return Ok(new
            {
                WorkOutPlan = workoutList.Select(we => new
                {
                    Name = we.workOutPlan.Name,
                    NumberOfDays = we.workOutPlan.NumberOfDays,
                    DifficultyLevel = we.workOutPlan.DifficultyLevel.ToString()
                }).FirstOrDefault(),
                Exercises = workoutList.Select(we => new
                {
                    Name = we.exercise.Name,
                    Description = we.exercise.Description,
                    Difficulty = we.exercise.Difficulty.ToString(),
                    TypeOfMachine = we.exercise.TypeOfMachine,
                    NumberOfReps = we.NumberOfReps,
                    NumberOfSets = we.NumberOfSets
                }).ToList()
            });
        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("WeightTracker")]
        public async Task<ActionResult> WeightTracker()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return BadRequest(new ApiValidationErrorResponse { Errors = new[] { "User Unauthorized" } });
            }
            var userMetrics = await _repoMetrics.GetAllAsync();
            var userMetric = userMetrics.Where(um => um.UserId == user.Id)
                .OrderByDescending(um => um.CreatedAt)
                .Select(u => u.Weight).ToList();
            if (!userMetric.Any())
            {
                return NotFound(new { message = "No weight data found for the user." });
            }
            else
                return Ok(userMetric);
        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("AddFood")]
        public async Task<ActionResult> AddFood(string FoodName, double Quantity)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return BadRequest(new ApiValidationErrorResponse { Errors = new[] { "User Unauthorized" } });
            }
            var food = await _fitGuideContext.Food.FirstOrDefaultAsync(f => f.Name == FoodName);
            if (food == null)
            {
                return BadRequest(new ApiValidationErrorResponse { Errors = new[] { "Food not found" } });
            }
            var foodLog =  _logFoodService.LogFood(user.Id, food.Id,Quantity);
            if (foodLog == null)
            {
                return BadRequest(new ApiValidationErrorResponse { Errors = new[] { "Failed to log food" } });
            }
            return Ok(new { Message = "Food logged successfully." });
        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("FoodDiary")]
        public async Task<ActionResult> FoodDiary(DateTime date)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return BadRequest(new ApiValidationErrorResponse { Errors = new[] { "User Unauthorized" } });
            }
            var logs = await _fitGuideContext.LogFood
                .Where(l => l.UserId == user.Id && l.LoggedAt.Date == DateTime.UtcNow.Date)
                .Include(l => l.foodItem)
                .ToListAsync();

            if (!logs.Any())
            {
                return NotFound("No logs found for the specified date.");
            }

            var result = logs.Select(log => new
            {
                Id = log.Id,
                FoodName = log.foodItem.Name,
                Description = log.foodItem.Description,
                Quantity = log.Quantity,
                Calories = (log.foodItem.CaloriesPerServing / 100) * log.Quantity,
                Protein = (log.foodItem.ProteinPerServing / 100) * log.Quantity,
                Carbs = (log.foodItem.CarbsPerServing / 100) * log.Quantity,
                Fat = (log.foodItem.FatPerServing / 100) * log.Quantity,
                LoggedAt = log.LoggedAt
            }).ToList();
            return Ok(result);

        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("DailySummary")]
        public async Task<ActionResult> GetDailySummary()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return BadRequest(new ApiValidationErrorResponse { Errors = new[] { "User Unauthorized" } });
            }
            var Nutrition=await _repoNutrition.GetAllAsync();
            var NutritionPlan= Nutrition.OrderByDescending(np => np.CreatedAt).FirstOrDefault(np => np.UserId == user.Id);
            if (NutritionPlan == null)
            {
                return BadRequest(new ApiValidationErrorResponse { Errors = new[] { "No nutrition plan found" } });
            }
            var TotalCalories=NutritionPlan.CaloriestTarget;
            var TotalProtein = NutritionPlan.ProteinTarget;
            var TotalCarbs = NutritionPlan.CarbsTarget;
            var TotalFat = NutritionPlan.FatTarget;

            var FoodIntake=await _genericDailyIntake.GetAllAsync();

            var foodIntake = FoodIntake.Where(f => f.UserId == user.Id&&f.Date==DateTime.UtcNow.Date)
                .FirstOrDefault();
            var caloriesIntake = foodIntake?.TotalCalories ?? 0;
            var proteinIntake = foodIntake?.TotalProtein ?? 0;
            var carbsIntake = foodIntake?.TotalCarbs ?? 0;
            var fatIntake = foodIntake?.TotalFat ?? 0;
            var remainingCalories = TotalCalories - caloriesIntake;

            return Ok(new
            {
                TotalCalories = TotalCalories,
                FoofIntake = caloriesIntake,
                remainingCalories = remainingCalories,
                TotalProtein= TotalProtein,
                FoodIntakeProtein = proteinIntake,
                TotalCarbs = TotalCarbs,
                FoodIntakeCarbs = carbsIntake,
                TotalFat = TotalFat,
                FoodIntakeFats = fatIntake,

            });

        }


    }
}
