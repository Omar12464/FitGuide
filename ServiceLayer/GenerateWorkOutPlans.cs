using Core;
using Core.Identity.Entities;
using Core.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Repository;
using Repository.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer
{
    public class GenerateWorkOutPlans
    {
        private readonly IGeneric<UserMetrics> _repoMetrics;
        private readonly FitGuideContext _fitGuideContext;
        private readonly IGeneric<WorkOutPlan> _repoWorkout;
        private readonly IGeneric<UserInjury> _repoInjury;

        public GenerateWorkOutPlans(IGeneric<UserMetrics> repoMetrics, FitGuideContext fitGuideContext, IGeneric<WorkOutPlan> repoWorkout, IGeneric<UserInjury> repoInjury)
        {
            _repoMetrics = repoMetrics;
            _fitGuideContext = fitGuideContext;
            _repoWorkout = repoWorkout;
            _repoInjury = repoInjury;
        }

        public async Task<List<WorkOutPlan>> GetWorkOut(string userId)
        {
            var workoutPlans = await _repoWorkout.GetAllAsync();
            var userMetrcs = await _fitGuideContext.userMetrics.FirstOrDefaultAsync(u => u.UserId.Equals(userId));
            var filteredworkouts = workoutPlans.Where(u => u.DifficultyLevel<=userMetrcs.fitnessLevel).ToList();

            return filteredworkouts;

        }
        public async Task<List<Exercise>> FilterExrcises(string userId)
        {
            var userMetrcs = await _fitGuideContext.userMetrics.FirstOrDefaultAsync(u => u.UserId.Equals(userId));
            var userInjuries = await _fitGuideContext.userInjuries
                           .Include(ui => ui.injury) // Include injury details
                           .Where(ui => ui.UserId == userId) // Only active injuries
                           .ToListAsync();
            if (!userInjuries.Any()) { return new List<Exercise>(); }
            var affectedBodyParts = userInjuries.Select(ui => ui.injury.AffectedBodyPart.ToLowerInvariant()).Distinct().ToList();
            var contraindicatedExercises = userInjuries
            .Where(i => affectedBodyParts.Contains(i.injury.AffectedBodyPart.ToLowerInvariant()))
            .SelectMany(i => i.injury.ContraindicatedExercises)
            .Distinct()
            .ToList();
            var query=_fitGuideContext.Exercise.AsQueryable();
            query = query.Where(e => !contraindicatedExercises.Contains(e.Name));
            var safeExercises =  userInjuries.Where(i => affectedBodyParts.Contains(i.injury.AffectedBodyPart.ToLowerInvariant())).SelectMany(i=>i.injury.ContraindicatedExercises).Distinct().ToList();
            if(safeExercises.Any()) { query = query.Where(e => safeExercises.Contains(e.TypeOfMachine)); }
            var filterexercise=await query.ToListAsync();
            var prioritizedExercises = filterexercise.OrderByDescending(e => e.Difficulty <= userMetrcs.fitnessLevel).ThenByDescending(e => affectedBodyParts.Contains(e.TargetMuscle.ToLowerInvariant())).ToList();
            
            
            return prioritizedExercises;

        }
        public async Task GeneratePersonalizedPlans(string userId,string PlanType)
        {
            var workout = await GetWorkOut(userId);
            var selectedWorkOut = workout.FirstOrDefault(t => t.Name.Equals(PlanType, StringComparison.OrdinalIgnoreCase));
            if (selectedWorkOut == null)
            {
                throw new ArgumentException($"No WorkOut available");
            }
            var filteredExercises = await FilterExrcises(userId);
            if (!filteredExercises.Any())
            {
                throw new ArgumentException($"No Suitable Exercises available");
            }
            var userInjuries = _fitGuideContext.userInjuries.Include(i => i.injury).Where(i => i.UserId == userId).ToList();
            var injuryAffectedParts = userInjuries
                .Select(ui => ui.injury.AffectedBodyPart.ToLowerInvariant())
                .Distinct()
                .ToList(); var dailyExercises = new Dictionary<string, List<Exercise>>();
            switch (PlanType.ToLowerInvariant())
            {
                case "pushpulllegs":
                    dailyExercises["Push Day"] = GetSafeExercisesForCategory(filteredExercises, new[] { "Chest", "Shoulders", "Triceps" },injuryAffectedParts);
                    dailyExercises["Pull Day"] = GetSafeExercisesForCategory(filteredExercises, new[] { "Back", "Biceps" }, injuryAffectedParts);
                    dailyExercises["Leg Day"] = GetSafeExercisesForCategory(filteredExercises, new[] { "Legs", "Glutes", "Hamstrings", "Quads" }, injuryAffectedParts);
                    break;

                case "upperlower":
                    dailyExercises["Upper Day"] = GetSafeExercisesForCategory(filteredExercises, new[] { "Chest", "Back", "Shoulders", "Biceps", "Triceps" }, injuryAffectedParts);
                    dailyExercises["Lower Day"] = GetSafeExercisesForCategory(filteredExercises, new[] { "Legs", "Glutes", "Hamstrings", "Quads" }, injuryAffectedParts);
                    break;
                default:
                    throw new ArgumentException($"Unsupported workout plan type: '{PlanType}'.");
            }
            foreach (var day in dailyExercises.Keys.ToList())
            {
                if (!dailyExercises[day].Any())
                {
                    if (!dailyExercises[day].Any())
                    {
                        throw new InvalidOperationException($"Failed to generate exercises for '{day}' in '{PlanType}' plan.");
                    }
                }
            }
        }

        

            private List<Exercise> GetSafeExercisesForCategory(IEnumerable<Exercise> exercises, IEnumerable<string> muscleGroups, IEnumerable<string> injuryAffectedParts)
            {
                 return exercises
                .Where(e => muscleGroups.Contains(e.TargetMuscle)) // Filter by muscle group
                .Where(e => !e.TargetInjury.Any(cip => injuryAffectedParts.Contains(cip.ToLowerInvariant()))) // Exclude contraindicated exercises
                .Take(5) // Limit the number of exercises per day
                .ToList();
            }
    }
}

