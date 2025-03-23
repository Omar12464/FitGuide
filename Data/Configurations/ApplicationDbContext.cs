using FitGuide.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;
using System.Reflection;

namespace FitGuide.Data.Configurations
{
    public class ApplicationDbContext: DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
         : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<UserMetrics> UserMetrics { get; set; }
        public DbSet<UserGoal> UserGoals { get; set; }
        public DbSet<WorkoutPlan> WorkoutPlans { get; set; }
        public DbSet<NutritionPlan> NutritionPlans { get; set; }
        public DbSet<Meal> Meals { get; set; }
        public DbSet<Food> Foods { get; set; }
        public DbSet<MealFood> MealFoods { get; set; }
        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<WorkoutSchedule> WorkoutSchedules { get; set; }
        public DbSet<WorkoutPlanExercise> WorkoutPlanExercises { get; set; }
        public DbSet<ExerciseFeedback> ExerciseFeedbacks { get; set; }
        public DbSet<Injury> Injuries { get; set; }
        public DbSet<Allergy> Allergies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Apply all Fluent API configurations automatically
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

    }
}
