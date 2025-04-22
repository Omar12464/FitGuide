using Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Repository
{
    public class FitGuideContextSeed
    {
        public static async Task SeedAsync(FitGuideContext fitGuideContext)
        {
            //if (await fitGuideContext.goalTempelates.AnyAsync()) return; // Skip if already seeded
            var options = new JsonSerializerOptions
            {
                MaxDepth = 64,
                PropertyNameCaseInsensitive = true,
                Converters = {new JsonStringEnumConverter()}
            };
            var goalData = File.ReadAllText("../Repository/JSONfiles/Goals.json");

            try
            {
                // Deserialize JSON data into a list of GoalTemplate objects
                var goals = JsonSerializer.Deserialize<List<GoalTempelate>>(goalData, options);

                if (goals == null || !goals.Any())
                {
                    Console.WriteLine("No valid GoalTemplate data found in the JSON file.");
                    return;
                }

                // Fetch existing GoalTemplate records from the database
                var existingGoals = await fitGuideContext.GoalTempelate.ToDictionaryAsync(g => g.name.ToLowerInvariant());

                foreach (var goal in goals)
                {
                    var goalNameLower = goal.name.ToLowerInvariant();

                    if (existingGoals.ContainsKey(goalNameLower))
                    {
                        // Update existing record if necessary
                        var existingGoal = existingGoals[goalNameLower];
                        existingGoal.description = goal.description ?? existingGoal.description;
                        existingGoal.targetWeight = goal.targetWeight ?? existingGoal.targetWeight;
                        existingGoal.targetMuscleMass = goal.targetMuscleMass ?? existingGoal.targetMuscleMass;
                        existingGoal.targetWaterMass = goal.targetWaterMass ?? existingGoal.targetWaterMass;
                    }
                    else
                    {
                        // Add new record if it doesn't exist
                        fitGuideContext.Set<GoalTempelate>().Add(goal);
                    }
                }

                // Save changes to the database
                await fitGuideContext.SaveChangesAsync();
                Console.WriteLine("GoalTemplate data successfully seeded.");
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Error deserializing JSON: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while seeding GoalTemplate data: {ex.Message}");
            }
            var injurydata = File.ReadAllText("../Repository/JSONfiles/Injury.json");
            var injuries = JsonSerializer.Deserialize<List<Injury>>(injurydata, options);
            var existedInjuries = await fitGuideContext.Injury.ToDictionaryAsync(injuries => injuries.Name.ToLowerInvariant());
            if (injurydata.Count() > 0)
            {
                if (fitGuideContext.Injury.Count() == 0 || !fitGuideContext.Injury.Any())
                {
                    foreach (var injury in injuries)
                    {
                        var injurytoLower = injury.Name.ToLowerInvariant();
                        if (existedInjuries.ContainsKey(injurytoLower))
                        {
                            var existedInjury = existedInjuries[injurytoLower];
                            existedInjury.AffectedBodyPart = injury.AffectedBodyPart ?? existedInjury.AffectedBodyPart;
                            existedInjury.SuitableEquipment = injury.SuitableEquipment ?? existedInjury.SuitableEquipment;
                            existedInjury.ContraindicatedExercises = injury.ContraindicatedExercises.ToList() ?? existedInjury.ContraindicatedExercises.ToList();
                            existedInjury.SuitableExercises = injury.SuitableExercises.ToList() ?? existedInjury.SuitableExercises.ToList();
                        }
                        else
                        {
                            fitGuideContext.Set<Injury>().Add(injury);
                        }
                    }
                    await fitGuideContext.SaveChangesAsync();
                }
            }
            var workoutdata = File.ReadAllText("../Repository/JSONfiles/WorkOut.json");
            var workoutPlans = JsonSerializer.Deserialize<List<WorkOutPlan>>(workoutdata, options);

            try
            {
                workoutPlans=JsonSerializer.Deserialize<List<WorkOutPlan>>(workoutdata, options); ;
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Error deserializing JSON: {ex.Message}");
            }
            if (workoutdata.Count() > 0)
            {
                if (fitGuideContext.WorkOutPlans.Count() == 0 || !fitGuideContext.WorkOutPlans.Any())
                {
                    foreach (var workoutPlan in workoutPlans)
                    {
                        fitGuideContext.Set<WorkOutPlan>().Add(workoutPlan);
                    }
                    await fitGuideContext.SaveChangesAsync();
                }
            }
            var exerciseData = File.ReadAllText("../Repository/JSONfiles/Exercise.json");
            try
            {
                var exercises = JsonSerializer.Deserialize<List<Exercise>>(exerciseData, options);

                // Fetch existing exercises from the database
                var existedExercises = await fitGuideContext.Exercise.ToDictionaryAsync(e => e.Name.ToLowerInvariant());

                if (exercises != null && exercises.Any())
                {
                    foreach (var exercise in exercises)
                    {
                        // Validate and populate missing fields
                        if (string.IsNullOrEmpty(exercise.TargetMuscle))
                        {
                            exercise.TargetMuscle = "General"; // Default value for unspecified muscles
                        }

                        if (exercise.TargetInjury == null || !exercise.TargetInjury.Any())
                        {
                            exercise.TargetInjury = new List<string> { "None" }; // Default value for no contraindications
                        }

                        if (string.IsNullOrEmpty(exercise.TypeOfMachine))
                        {
                            exercise.TypeOfMachine = "Bodyweight"; // Default value for unspecified equipment
                        }

                        // Check if the exercise already exists in the database
                        var exerciseNameLower = exercise.Name.ToLowerInvariant();
                        if (existedExercises.ContainsKey(exerciseNameLower))
                        {
                            var existingExercise = existedExercises[exerciseNameLower];

                            // Update existing exercise with non-null values
                            existingExercise.TypeOfMachine = exercise.TypeOfMachine ?? existingExercise.TypeOfMachine;
                            existingExercise.TargetInjury = exercise.TargetInjury ?? existingExercise.TargetInjury;
                            existingExercise.TargetMuscle = exercise.TargetMuscle ?? existingExercise.TargetMuscle;
                        }
                        else
                        {
                            // Add new exercise to the database
                            fitGuideContext.Set<Exercise>().Add(exercise);
                        }
                    }

                    // Save changes to the database
                    await fitGuideContext.SaveChangesAsync();
                }
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Error deserializing JSON: {ex.Message}");
            }

        }
 }
}
