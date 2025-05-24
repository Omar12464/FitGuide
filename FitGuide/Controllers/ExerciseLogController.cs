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
using Microsoft.EntityFrameworkCore;
using Repository;

namespace FitGuide.Controllers
{

    public class ExerciseLogController : BaseAPI
    {
        private readonly FitGuideContext _fitGuideContext;
        private readonly IGeneric<WorkOutExercises> _repoWorkoutEx;
        private readonly UserManager<User> _userManager;
        private readonly IGeneric<ExerciseLog> _repoLog;
        private readonly IGeneric<Exercise_Feedback> _repoFeedback;

        public ExerciseLogController(FitGuideContext fitGuideContext, IGeneric<WorkOutExercises> repoWorkoutEx, UserManager<User> userManager, IGeneric<ExerciseLog> repoLog, IGeneric<Exercise_Feedback> repoFeedback)
        {
            _fitGuideContext = fitGuideContext;
            _repoWorkoutEx = repoWorkoutEx;
            _userManager = userManager;
            _repoLog = repoLog;
            _repoFeedback = repoFeedback;
        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("GetExerciseDetails")]
        public async Task<ActionResult> GetExerciseDeatils(int WorkOutExerciseID)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return BadRequest(new ApiValidationErrorResponse { Errors = new[] { "User Unauthorized" } });
            }
            if (WorkOutExerciseID == null)
            {
                return BadRequest(new ApiValidationErrorResponse { Errors = new[] { "Invalid exercise details provided." } });
            }
            var exerciseDetails = await _fitGuideContext.workOutExercises.Where(e => e.Id == WorkOutExerciseID && e.UserId == user.Id).Include(e => e.exercise)
                .Select(e => new ExerciseDetailsDTO
                {
                    ExerciseName = e.exercise.Name,
                    Reps = e.NumberOfReps,
                    Sets = e.NumberOfSets,
                    MaxWeight = e.Weight,
                }).FirstOrDefaultAsync();

            if (exerciseDetails == null)
            {
                return NotFound(new ApiValidationErrorResponse { Errors = new[] { "Exercise details not found." } });
            }
            return Ok(exerciseDetails);


        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("Save Feedback/{Id}")]
        public async Task<ActionResult> Save([FromBody] FeedbackDTO feedbackDTO)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return BadRequest(new ApiValidationErrorResponse { Errors = new[] { "User Unauthorized" } });
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiValidationErrorResponse { Errors = new[] { "MODE IS NOT VALID." } });

            }
            if (feedbackDTO == null )
            {
                return BadRequest(new ApiValidationErrorResponse { Errors = new[] { "Invalid feedback data provided." } });
            }
            var exerciseLog = await _fitGuideContext.workOutExercises
                .FirstOrDefaultAsync(e => e.Id == feedbackDTO.WorKoutExerciseId && e.UserId == user.Id);
            if (exerciseLog == null)
            {
                return NotFound(new ApiValidationErrorResponse { Errors = new[] { "Exercise log not found." } });
            }
            byte[] videoFeedbacks = feedbackDTO.VideoFeedback64 != null ? Convert.FromBase64String(feedbackDTO.VideoFeedback64) : null;


            var Exerciselog = new ExerciseLog
            {
                UserId = user.Id,
                WorkOutExerciseId = exerciseLog.Id,
                VideoFeedback = videoFeedbacks,
                LoggedAt = DateTime.UtcNow,
            };
            await _repoLog.AddAsync(Exerciselog);
            foreach (var Feedbac in feedbackDTO.FeedbackText)
            {
                var feedback = new Exercise_Feedback
                {
                    ExerciseLogId = Exerciselog.Id,
                    ExerciseFeedback = Feedbac,
                };
                await _repoFeedback.AddAsync(feedback);
            }
            return Ok(new { Message = $"Feedback for {user.FistName} saved successfully." });

        }
    }
}
