using AutoMapper;
using Core;
using Core.Identity.Entities;
using Core.Interface;
using Core.Interface.Services;
using FitGuide.DTOs;
using FitGuide.ErrorsManaged;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repository;

namespace FitGuide.Controllers
{
    public class WorkOutController : BaseAPI
    {
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly IGenrateWorkOutService _genrateWorkOutService;
        private readonly FitGuideContext _fitGuideContext;
        private readonly IGeneric<WorkOutExercises> _repoWorkoutExercise;

        public WorkOutController(
            IMapper mapper,
            UserManager<User> userManager,
            IGenrateWorkOutService genrateWorkOutService,
            FitGuideContext fitGuideContext,
            IGeneric<WorkOutExercises> repoWorkoutExercise)
        {
            _mapper = mapper;
            _userManager = userManager;
            _genrateWorkOutService = genrateWorkOutService;
            _fitGuideContext = fitGuideContext;
            _repoWorkoutExercise = repoWorkoutExercise;
        }

        [HttpGet("Show WorkOut Plans")]
        public async Task<ActionResult> GetAllWorkOutPlans()
        {
            var workoutplans=await _fitGuideContext.WorkOutPlans.ToListAsync();
            return Ok(workoutplans);
        }
        

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("GenerateWorkOut")]
        public async Task<ActionResult> GenerateWorkOutPlan(string planType)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return BadRequest(new ApiValidationErrorResponse { Errors = new[] { "User UnAuthorized" } });
            }
            var workoutexercsie = await _fitGuideContext.workOutExercises.Where(we => user.Id == we.UserId && we.IsActive == true).ToListAsync();
            if (workoutexercsie.Any())
            {
                return BadRequest(new ApiValidationErrorResponse { Errors = new[] { "User has already have a workoutplan" } });
            }
            try
            {
                await _genrateWorkOutService.GeneratePersonalizedPlans(user.Id, planType);

                return Ok(new { Message = "Workout plan generated successfully." });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ApiValidationErrorResponse { Errors = new[] { ex.Message } });
            }
        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut("updateWorkOut")]
        public async Task<ActionResult> UpdateWorkOutPlan(string planType)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return BadRequest(new ApiValidationErrorResponse { Errors = new[] { "User UnAuthorized" } });
            }
            var workoutexercsie = await _repoWorkoutExercise.GetAllAsync();
            if(!workoutexercsie .Any() )
            {
                return BadRequest(new ApiValidationErrorResponse { Errors = new[] { "No Workout Plans already generated" } });
            }
            var worksduplicate = workoutexercsie.Where(u => user.Id.Equals(u.UserId) && u.IsActive == true);
            var currentPlanType = worksduplicate.FirstOrDefault()?.workOutPlan?.Name; // Get the name of the current active plan
            if (currentPlanType != null && currentPlanType.Equals(planType, StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest(new ApiValidationErrorResponse { Errors = new[] { $"You are already on the '{planType}' workout plan." } });
            }
            foreach (var exercise in workoutexercsie)
            {
                exercise.IsActive = false;
            }
            try
            {
                await _genrateWorkOutService.GeneratePersonalizedPlans(user.Id, planType);

                return Ok(new { Message = "Workout plan generated successfully." });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ApiValidationErrorResponse { Errors = new[] { ex.Message } });
            }
        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("GetMyWorkOutPlan")]
        public async Task<ActionResult> GetWorkoutPlans()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return BadRequest(new ApiValidationErrorResponse { Errors = new[] { "User UnAuthorized" } });
            }
            // Fetch the raw data from the database
            var workouts = await _fitGuideContext.workOutExercises
                .Where(we => we.UserId == user.Id&&we.IsActive== true)
                .Include(we => we.workOutPlan)
                .OrderByDescending(we => we.workOutPlan)
                .Include(we => we.exercise)
                .ToListAsync();

            if (!workouts.Any())
            {
                return NotFound(new { message = "No workout plans found for the user." });
            }

            // Group exercises by workout plan
            var groupedWorkouts = workouts
                .Where(we=>we.IsActive==true)
                .GroupBy(we => we.workOutPlan.Id)// Group by workout plan ID
                .Select(group => new
                {
                    UserName = user.FullName,
                    WorkOutPlan = new
                    {
                        Name = group.First().workOutPlan.Name,
                        Description = group.First().workOutPlan.Description,
                        NumberOfDays = group.First().workOutPlan.NumberOfDays,
                        DifficultyLevel = group.First().workOutPlan.DifficultyLevel.ToString()
                    },
                    Exercises = group.Select(we => new
                    {
                        ExerciseId = we.ExerciseId,
                        Name = we.exercise.Name,
                        Description = we.exercise.Description,
                        Difficulty = we.exercise.Difficulty.ToString(),
                        TypeOfMachine = we.exercise.TypeOfMachine,
                        TargetMuscle = we.exercise.TargetMuscle,
                        TargetInjury = we.exercise.TargetInjury,
                        NumberOfReps = we.NumberOfReps,
                        NumberOfSets = we.NumberOfSets
                    }).ToList()
                })
                .ToList();

            return Ok(groupedWorkouts);
        }
    }
}