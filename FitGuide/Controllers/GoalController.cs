using AutoMapper;
using Core;
using Core.Identity.Entities;
using Core.Interface;
using Core.Interface.Services;
using FitGuide.DTOs;
using FitGuide.ErrorsManaged;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repository;
using ServiceLayer;

namespace FitGuide.Controllers
{

    public class GoalController : BaseAPI
    {
        private readonly FitGuideContext _fitGuideContext;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly IGeneric<UserGoal> _repoGoal;
        private readonly IGeneric<GoalTempelate> _repoGoalTemplate;
        private readonly IUserMetricsServices _userMetrics;
        private readonly IGeneric<UserMetrics> _repo;
        private readonly IWeightCategory _weightCategoryRanges;
        private readonly IWeightTarget _weightCategoryTargets;

        public GoalController(FitGuideContext fitGuideContext, UserManager<User> userManager, IMapper mapper, IGeneric<UserGoal> RepoGoal, IGeneric<GoalTempelate> RepoGoalTemplate,IUserMetricsServices userMetrics,IGeneric<UserMetrics>repo,IWeightCategory weightCategoryRanges, IWeightTarget weightCategoryTargets)
        {  _fitGuideContext = fitGuideContext;
            _userManager = userManager;
            _mapper = mapper;
            _repoGoal = RepoGoal;
            _repoGoalTemplate = RepoGoalTemplate;
            _userMetrics = userMetrics;
            _repo = repo;
            _weightCategoryRanges = weightCategoryRanges;
            _weightCategoryTargets = weightCategoryTargets;
        }

        [HttpGet("GetAllGoals")]
        public async Task<ActionResult> GetAllGoals()
        {
            var goals=await _repoGoalTemplate.GetAllAsync();
            var goalName=goals.Select(g=>g.name).ToList();
            return Ok(goalName);
        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("SelectGoal")]
        public async Task<ActionResult> SelectGoal(string GoalName)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return BadRequest(new ApiValidationErrorResponse() { Errors = new string[] { "User UnAuthorized" } });
            }
            var userGoals=await _fitGuideContext.userGoals.Where(u=>u.UserId.Equals(user.Id)&&u.IsActive).ToListAsync();
            var usergoal = userGoals.Select(g => g.name);
            if (usergoal.Contains(GoalName))
            {
                return BadRequest(new ApiValidationErrorResponse() { Errors = new string[] { "User has already selected this goal" } });
            }
            // Make the current usergoal to be Isactive =false to avoid duplication
            foreach (var userGoal in userGoals)
            {
                userGoal.IsActive = false;
                _repoGoal.UpdateAsync(userGoal);
            }
            var userMetricsAll = await _repo.GetAllAsync();
            var userMetrics = userMetricsAll.OrderByDescending(w=>w.CreatedAt).FirstOrDefault();
            if (userMetrics == null)
            {
                return NotFound(new ApiExceptionResponse(404, "User metrics not found."));
            }

            if (string.IsNullOrEmpty(GoalName))
            {
                return BadRequest(new ApiExceptionResponse(400, "Goal name is required."));
            }

            var goalTemplate = await _repoGoalTemplate.GetFirstAsync(g => g.name == GoalName);
            if (goalTemplate == null)
            {
                return NotFound(new ApiExceptionResponse(404, "Goal template not found."));
            }

            // Get weight category and its default target values
            var bmi = userMetrics.BMI ?? 0;
            var weightCat = userMetrics.weightCategory;
            var weightCatTarget = _weightCategoryTargets.GetTargetForCategory(weightCat);

            // Calculate base target values from weight category
            float baseTargetBMI = weightCatTarget.TargetBMI;

            // Calculate target weight based on desired BMI
            float desiredBMI = baseTargetBMI; // Default to weight category target BMI
            switch (GoalName.ToLower())
            {
                case "weight loss":
                    desiredBMI = Math.Min(baseTargetBMI, 22.0f); // Lower BMI for weight loss
                    break;

                case "muscle gain":
                    desiredBMI = Math.Max(baseTargetBMI, 24.0f); // Higher BMI for muscle gain
                    break;

                case "endurance improvement":
                    desiredBMI = 22.5f; // Ideal BMI for endurance
                    break;

                case "injury recovery (lower body)":
                case "injury recovery (upper body)":
                case "mobility and flexibility":
                case "general health maintenance":
                    desiredBMI = 23.0f; // Moderate BMI for general health
                    break;

                case "strength training for beginners":
                    desiredBMI = 23.5f; // Slightly higher BMI for strength training
                    break;

                case "post-pregnancy fitness":
                    desiredBMI = 24.0f; // Gradual weight adjustment
                    break;

                case "athletic performance enhancement":
                    desiredBMI = 23.0f; // Leaner physique for athletic performance
                    break;

                default:
                    desiredBMI = baseTargetBMI; // Use default if goal not recognized
                    break;
            }

            // Calculate target weight using BMI formula
            float heightInMeters = userMetrics.Height / 100.0f; // Convert height from cm to meters
            float targetWeight = desiredBMI * (heightInMeters * heightInMeters);

            // Calculate weight change
            float weightChange =Math.Abs(targetWeight - userMetrics.Weight);
            weightChange = (weightChange/userMetrics.Weight) * 100;

            // Distribute weight change into muscle mass, fat, and water mass based on goal
            float targetFatChange=userMetrics.Fat??0, targetMuscleMassChange, targetWaterMassChange;
            float targetFatPercentage =0;
            switch (GoalName.ToLower())
            {
                case "weight loss":
                    targetFatPercentage -= 10; // Focus on fat loss
                    //targetMuscleMassChange = weightChange > 0 ? weightChange * 0.3f : 0; // Preserve muscle
                    //targetWaterMassChange = weightChange > 0 ? weightChange * 0.1f : 0; // Minimal water change
                    break;

                case "muscle gain":
                    targetFatPercentage -= 5; 
                    //targetMuscleMassChange = weightChange > 0 ? weightChange * 0.8f : 0; // Focus on muscle gain
                    //targetWaterMassChange = weightChange > 0 ? weightChange * 0.1f : 0; // Minimal water change
                    break;

                case "endurance improvement":
                    targetFatPercentage -= 5;
                    //targetMuscleMassChange = weightChange > 0 ? weightChange * 0.4f : 0; // Balanced muscle gain
                    //targetWaterMassChange = weightChange > 0 ? weightChange * 0.4f : 0; // Higher water mass
                    break;

                case "injury recovery (lower body)":
                case "injury recovery (upper body)":
                    targetFatPercentage -= 10; 
                    //targetMuscleMassChange = weightChange > 0 ? weightChange * 0.3f : 0; // Focus on muscle retention
                    //targetWaterMassChange = weightChange > 0 ? weightChange * 0.2f : 0; // Good hydration
                    break;

                case "mobility and flexibility":
                    targetFatPercentage -= 4;
                    //targetMuscleMassChange = weightChange > 0 ? weightChange * 0.2f : 0;  Minimal muscle gain
                    //targetWaterMassChange = weightChange > 0 ? weightChange * 0.2f : 0; // Balanced water mass
                    break;

                case "general health maintenance":
                    targetFatPercentage -= 5;
                    //targetMuscleMassChange = weightChange > 0 ? weightChange * 0.5f : 0;  Balanced muscle gain
                    //targetWaterMassChange = weightChange > 0 ? weightChange * 0.2f : 0; // Balanced water mass
                    break;

                case "strength training for beginners":
                    targetFatPercentage -= 5;
                    //targetMuscleMassChange = weightChange > 0 ? weightChange * 0.7f : 0; Focus on muscle gain
                    //targetWaterMassChange = weightChange > 0 ? weightChange * 0.1f : 0; // Minimal water change
                    break;

                case "post-pregnancy fitness":
                    targetFatPercentage -= 10; // Moderate fat loss
                    //targetMuscleMassChange = weightChange > 0 ? weightChange * 0.3f : 0; // Balanced muscle gain
                    //targetWaterMassChange = weightChange > 0 ? weightChange * 0.2f : 0; // Balanced water mass
                    break;

                case "athletic performance enhancement":
                    targetFatPercentage -= 8; // Minimal fat gain
                    //targetMuscleMassChange = weightChange > 0 ? weightChange * 0.8f : 0; // Focus on muscle gain
                    //targetWaterMassChange = weightChange > 0 ? weightChange * 0.1f : 0; // Minimal water change
                    break;

                default:
                    targetFatPercentage -= 3; // Default fat change
                    //targetMuscleMassChange = weightChange > 0 ? weightChange * 0.5f : 0; // Default muscle gain
                    //targetWaterMassChange = weightChange > 0 ? weightChange * 0.2f : 0; // Default water change
                    break;
            }

            // Calculate absolute target values
            targetFatChange += targetFatPercentage;
            // Ensure target fat percentage is realistic
            targetFatChange = Math.Max(targetFatChange, 5.0f); // Minimum healthy fat percentage

            // Calculate fat mass
            float absoluteTargetFat = targetWeight * (targetFatChange / 100);

            // Calculate fat-free mass (FFM)
            float fatFreeMass = targetWeight - absoluteTargetFat;

            // Calculate water mass (73% of FFM)
            float absoluteTargetWaterMass = fatFreeMass * 0.73f;

            // Calculate muscle mass (remaining portion of FFM after accounting for water)
            float absoluteTargetMuscleMass = fatFreeMass - absoluteTargetWaterMass;

            // Safety checks - ensure targets are within healthy ranges
            absoluteTargetFat = Math.Max(absoluteTargetFat, 5.0f); // Minimum healthy body fat
            absoluteTargetMuscleMass = Math.Max(absoluteTargetMuscleMass, 0); // No negative muscle mass
            absoluteTargetWaterMass = Math.Max(absoluteTargetWaterMass, 0); // No negative water mass

            // Ensure the total equals the target weight
            float totalMass = absoluteTargetFat + absoluteTargetMuscleMass + absoluteTargetWaterMass;
            if (totalMass != targetWeight)
            {
                absoluteTargetWaterMass += targetWeight - totalMass; // Adjust water mass to balance the total
            }

            // Create the user goal
            var userG = new UserGoal
            {
                UserId = user.Id,
                CreatedAt = DateTime.UtcNow,
                targetBMI = desiredBMI,
                description = $"Goal made based on {user.FistName}'s metrics and selected goal: {GoalName}",
                name = goalTemplate.name,
                targetMuscleMass = absoluteTargetMuscleMass,
                targetWaterMass = absoluteTargetWaterMass,
                targetWeight = targetWeight,
                targeFat = absoluteTargetFat,
                IsActive = true

            };

            await _repoGoal.AddAsync(userG);

            // Return the response
            return Ok(new UserGoalDTO
            {
                name = userG.name,
                targetWeight = userG.targetWeight.Value,
                targetMuscleMass = userG.targetMuscleMass.Value,
                targetWaterMass = userG.targetWaterMass.Value,
                targetBMI = desiredBMI,
                targetFat = userG.targeFat,
                description = userG.description
            });
        }


        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpDelete("RemoveGoal")]
        public async Task<ActionResult> RemoveGoal()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return BadRequest(new ApiValidationErrorResponse() { Errors = new string[] { "User UnAuthorized" } });
            }
            var usergoal = await _repoGoal.GetFirstAsync(ug => ug.IsActive == true && ug.UserId.Equals(user.Id));
            if (usergoal == null)
            {
                return BadRequest(new ApiValidationErrorResponse() { Errors = new string[] { "Goal Not Related To You" } });
            }
            try
            {
             _repoGoal.DeleteAsync(usergoal);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiValidationErrorResponse() { Errors = new string[] { $"Failed to delete the goal:: {ex.Message}" } });
            }

            return Ok(new
            {
                Message = $"Your Goal {usergoal.name} has been deleted"
            });
        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("GetUserGoal")]
        public async Task<ActionResult> GetGoal()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return BadRequest(new ApiValidationErrorResponse() { Errors = new string[] { "User UnAuthorized" } });
            }
            var userGoal =await _repoGoal.GetFirstAsync(u=>u.IsActive&&u.UserId.Equals(user.Id));
            var UserGoal= new UserGoalDTO
            {   
                name=userGoal.name,
                targetMuscleMass=userGoal.targetMuscleMass,
                targetBMI=userGoal.targetBMI,
                targetFat=userGoal.targeFat,
                targetWaterMass=userGoal.targetWaterMass,
                targetWeight=userGoal.targetWeight,
                description=userGoal.description
            };
            return Ok(new {
               Description= $"Goal For{user.FistName}:",
                UserGoal
            });

        }


    }
}
