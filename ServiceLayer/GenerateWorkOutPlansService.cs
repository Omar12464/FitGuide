using Core;
using Core.Identity.Entities;
using Core.Interface;
using Core.Interface.Services;
using Microsoft.AspNetCore.Identity;
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
    public class GenerateWorkOutPlansService:IGenrateWorkOutService
    {
        private readonly IGeneric<UserMetrics> _repoMetrics;
        private readonly FitGuideContext _fitGuideContext;
        private readonly IGeneric<WorkOutPlan> _repoWorkout;
        private readonly IGeneric<UserInjury> _repoInjury;
        private readonly IGeneric<WorkOutExercises> _repoWorkExercise;
        private readonly UserManager<User> _userManager;

        public GenerateWorkOutPlansService(IGeneric<UserMetrics> repoMetrics, FitGuideContext fitGuideContext, IGeneric<WorkOutPlan> repoWorkout, IGeneric<UserInjury> repoInjury,IGeneric<WorkOutExercises> repoWorkExercise,UserManager<User> user)
        {
            _repoMetrics = repoMetrics;
            _fitGuideContext = fitGuideContext;
            _repoWorkout = repoWorkout;
            _repoInjury = repoInjury;
            _repoWorkExercise = repoWorkExercise;
            _userManager = user;
        }

        public async Task<List<WorkOutPlan>> GetWorkOut(string userId)
        {
            var workoutPlans = await _repoWorkout.GetAllAsync();
            var userMetrcs = await _repoMetrics.GetFirstAsync(u => u.UserId.Equals(userId));
            var filteredworkouts = workoutPlans.Where(u => u.DifficultyLevel <= userMetrcs.fitnessLevel).ToList();


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
            var safeExercises =  userInjuries.Where(i => affectedBodyParts.Contains(i.injury.AffectedBodyPart.ToLowerInvariant())).SelectMany(i=>i.injury.SuitableEquipment).Distinct().ToList();
            if(safeExercises.Any()) { query = query.Where(e => safeExercises.Contains(e.TypeOfMachine)); }
            var filterexercise=await query.ToListAsync();
            var prioritizedExercises = filterexercise.OrderByDescending(e => e.Difficulty <= userMetrcs.fitnessLevel).ThenByDescending(e => !affectedBodyParts.Contains(e.TargetMuscle.ToLowerInvariant())).ToList();
            
            
            return prioritizedExercises;

        }
        public async Task GeneratePersonalizedPlans(string userId,string PlanType)
        {
            var workout = await GetWorkOut(userId);
            var user= await _userManager.FindByIdAsync(userId);
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
                .ToList(); 
            var dailyExercises = new Dictionary<string, List<Exercise>>();
            switch (PlanType)
            {
                case "Push Pull Legs":
                    dailyExercises["Push Day"] = GetSafeExercisesForCategory(filteredExercises, new[] { "Chest", "Shoulders", "Triceps" }, injuryAffectedParts);
                    dailyExercises["Pull Day"] = GetSafeExercisesForCategory(filteredExercises, new[] { "Back", "Biceps" }, injuryAffectedParts);
                    dailyExercises["Leg Day"] = GetSafeExercisesForCategory(filteredExercises, new[] { "Legs", "Glutes", "Hamstrings", "Quads" }, injuryAffectedParts);
                    break;

                case "Upper Lower Split":
                    dailyExercises["Upper Day"] = GetSafeExercisesForCategory(filteredExercises, new[] { "Chest", "Back", "Shoulders", "Biceps", "Triceps" }, injuryAffectedParts);
                    dailyExercises["Lower Day"] = GetSafeExercisesForCategory(filteredExercises, new[] { "Legs", "Glutes", "Hamstrings", "Quads" }, injuryAffectedParts);
                    break;

                case "Full Body Workout":
                    dailyExercises["Full Body"] = GetSafeExercisesForCategory(filteredExercises, new[] { "Chest", "Back", "Legs", "Shoulders", "Biceps", "Triceps", "Core" }, injuryAffectedParts);
                    break;

                case "Strength Training Plan":
                    dailyExercises["Day 1 - Upper Body"] = GetSafeExercisesForCategory(filteredExercises, new[] { "Chest", "Triceps", "Shoulders" }, injuryAffectedParts);
                    dailyExercises["Day 2 - Lower Body"] = GetSafeExercisesForCategory(filteredExercises, new[] { "Legs", "Glutes", "Hamstrings", "Quads" }, injuryAffectedParts);
                    dailyExercises["Day 3 - Back & Biceps"] = GetSafeExercisesForCategory(filteredExercises, new[] { "Back", "Biceps" }, injuryAffectedParts);
                    dailyExercises["Day 4 - Core & Stability"] = GetSafeExercisesForCategory(filteredExercises, new[] { "Core", "Lower Back" }, injuryAffectedParts);
                    break;

                case "Endurance Training Plan":
                    dailyExercises["Day 1 - Push & Core"] = GetSafeExercisesForCategory(filteredExercises, new[] { "Chest", "Shoulders", "Triceps", "Core" }, injuryAffectedParts);
                    dailyExercises["Day 2 - Pull & Cardio"] = GetSafeExercisesForCategory(filteredExercises, new[] { "Back", "Biceps" }, injuryAffectedParts);
                    dailyExercises["Day 3 - Legs & Endurance"] = GetSafeExercisesForCategory(filteredExercises, new[] { "Legs", "Glutes", "Hamstrings", "Quads" }, injuryAffectedParts);
                    dailyExercises["Day 4 - Functional Movements"] = GetSafeExercisesForCategory(filteredExercises, new[] { "Core", "Forearms", "Lower Back" }, injuryAffectedParts);
                    dailyExercises["Day 5 - Active Recovery"] = GetSafeExercisesForCategory(filteredExercises, new[] { "Stretching", "Mobility" }, injuryAffectedParts);
                    break;

                case "Hypertrophy Training Plan":
                    dailyExercises["Day 1 - Chest & Triceps"] = GetSafeExercisesForCategory(filteredExercises, new[] { "Chest", "Triceps" }, injuryAffectedParts);
                    dailyExercises["Day 2 - Back & Biceps"] = GetSafeExercisesForCategory(filteredExercises, new[] { "Back", "Biceps" }, injuryAffectedParts);
                    dailyExercises["Day 3 - Legs"] = GetSafeExercisesForCategory(filteredExercises, new[] { "Legs", "Glutes", "Hamstrings", "Quads" }, injuryAffectedParts);
                    dailyExercises["Day 4 - Shoulders & Core"] = GetSafeExercisesForCategory(filteredExercises, new[] { "Shoulders", "Core" }, injuryAffectedParts);
                    break;

                case "Rehabilitation Plan":
                    dailyExercises["Day 1 - Stretching & Mobility"] = GetSafeExercisesForCategory(filteredExercises, new[] { "Stretching", "Mobility" }, injuryAffectedParts);
                    dailyExercises["Day 2 - Low-Impact Cardio"] = GetSafeExercisesForCategory(filteredExercises, new[] { "Cardio", "Core Activation" }, injuryAffectedParts);
                    dailyExercises["Day 3 - Rehabilitation Exercises"] = GetSafeExercisesForCategory(filteredExercises, new[] { "Rehabilitation", "Dynamic Stretching" }, injuryAffectedParts);
                    break;

                case "CrossFit-Inspired Plan":
                    dailyExercises["Day 1 - Push & Pull"] = GetSafeExercisesForCategory(filteredExercises, new[] { "Chest", "Shoulders", "Triceps", "Back", "Biceps" }, injuryAffectedParts);
                    dailyExercises["Day 2 - Legs & Core"] = GetSafeExercisesForCategory(filteredExercises, new[] { "Legs", "Glutes", "Hamstrings", "Quads", "Core" }, injuryAffectedParts);
                    dailyExercises["Day 3 - Functional Movements"] = GetSafeExercisesForCategory(filteredExercises, new[] { "Core", "Functional Movements" }, injuryAffectedParts);
                    dailyExercises["Day 4 - High-Intensity Cardio"] = GetSafeExercisesForCategory(filteredExercises, new[] { "Cardio", "Plyometrics" }, injuryAffectedParts);
                    dailyExercises["Day 5 - Mixed Compound Lifts"] = GetSafeExercisesForCategory(filteredExercises, new[] { "Chest", "Back", "Legs", "Shoulders" }, injuryAffectedParts);
                    dailyExercises["Day 6 - Endurance Circuits"] = GetSafeExercisesForCategory(filteredExercises, new[] { "Endurance", "Circuits" }, injuryAffectedParts);
                    break;

                default:
                    throw new ArgumentException($"Unsupported workout plan type: '{PlanType}'.");
            }
            foreach (var day in dailyExercises.Keys.ToList())
            {
   
                    if (!dailyExercises[day].Any())
                    {
                        throw new InvalidOperationException($"Failed to generate exercises for '{day}' in '{PlanType}' plan.");
                    }
            }
            foreach (var (day, exercises) in dailyExercises)
            {
                foreach (var exercise in exercises)
                {
                    //int numberOfDays=3;
                    //if (day.Equals(1)) { numberOfDays = 1; }else if (day.Equals(2)) { numberOfDays = 2; }else if (day.Equals(3)) { numberOfDays = 3; }
                    var workooutexercises = new WorkOutExercises
                    {
                        WorkOutName = $"This WorkOutPlan has been generated for {user.FullName} based on his inputs",
                        WorkoOutId = selectedWorkOut.Id,
                        ExerciseId = exercise.Id,
                        NumberOfSets = 3,
                        NumberOfReps = 10,
                        UserId = userId

                    };
                   await _repoWorkExercise.AddAsync(workooutexercises);
                }
            }
        }

        

            public List<Exercise> GetSafeExercisesForCategory(IEnumerable<Exercise> exercises, IEnumerable<string> muscleGroups, IEnumerable<string> injuryAffectedParts)
            {
                 return exercises
                .Where(e => muscleGroups.Contains(e.TargetMuscle)) // Filter by muscle group
                .Where(e => !e.TargetInjury.Any(cip => injuryAffectedParts.Contains(cip.ToLowerInvariant())))
                .OrderBy(e=>Guid.NewGuid())// Exclude contraindicated exercises // \
                .ToList();
            }
        public async Task<WorkOutExercises> GetWorkOutsPlansForUser(string userId)
        {
            var workouts =await _repoWorkExercise.GetFirstAsync(workout=>workout.UserId==userId);
            return workouts;
        }
        public async Task<List<WorkOutExercises>> GetAllUserWorkouts(string userId)
        {
            var workouts = await _repoWorkExercise.GetAllAsync();
            var userWorkout= await _fitGuideContext.Set<WorkOutExercises>().Include(u=>u.workOutPlan).Include(e=>e.exercise).OrderBy(e=>e.WorkOutName).ToListAsync();
            return userWorkout;  
        }
    }
}

