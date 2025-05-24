using Core;
using Core.Identity.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Repository
{
    public class FitGuideContext : DbContext
    {
        private readonly DbContextOptions<FitGuideContext> _options;

        public FitGuideContext(DbContextOptions<FitGuideContext> options) : base(options)
        {
            _options = options;
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ExerciseLog>().HasOne(e=>e.workOutExercises).WithOne(w => w.exerciseLog).HasForeignKey<ExerciseLog>(e => e.WorkOutExerciseId);
            modelBuilder.Entity<ExerciseLog>().HasMany(e=>e.exercise_Feedback).WithOne(e=>e.exerciseLog).HasForeignKey(e=>e.ExerciseLogId);
            modelBuilder.Entity<ExerciseLog>(entity =>
            {
                entity.HasKey
                (el => el.Id);
                entity.Property(el => el.LoggedAt).HasColumnType("datetime");

            });
       
            modelBuilder.Entity<FoodItem>(entity =>
            {
                entity.HasKey(f => f.Id);

                entity.Property(f => f.Name).IsRequired().HasMaxLength(200);

                entity.Property(f => f.CarbsPerServing).HasColumnType("decimal(10,2)");
                entity.Property(f => f.ProteinPerServing).HasColumnType("decimal(10,2)");
                entity.Property(f => f.CarbsPerServing).HasColumnType("decimal(10,2)");
                entity.Property(f => f.FatPerServing).HasColumnType("decimal(10,2)");

            });
            modelBuilder.Entity<LogFood>(entity =>
            {
                entity.Property(lf => lf.Quantity).HasColumnType("decimal(10,2)");
                entity.Property(lf => lf.LoggedAt).HasColumnType("datetime");
                entity.HasOne(lf => lf.foodItem).WithMany(f=>f.logFoods).HasForeignKey(lf => lf.FoodId);
            });
            modelBuilder.Entity<UserMetrics>(e =>
            {
                e.Property(u => u.weightCategory).HasConversion<string>();
                e.Property(u => u.fitnessLevel).HasConversion<string>();
                e.Property(u => u.GymFrequency).HasConversion<string>();

            });
            modelBuilder.Entity<Exercise>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e=>e.Difficulty).HasConversion<string>();
                entity.Property(e => e.TypeOfMachine).IsRequired(false).HasMaxLength(1000);
                entity.Property(e => e.TargetMuscle).IsRequired(false).HasMaxLength(1000);
                entity.HasMany(w => w.workOutExercises).WithOne(w => w.exercise).HasForeignKey(w => w.ExerciseId);
                //entity.Property(e => e.GifBytes).HasColumnType("varbinary(max)");
                //entity.Property(e => e.GifPath).HasMaxLength(50);

                entity.Property(e => e.TargetInjury).HasConversion(
                 v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                 v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions)null) ?? new List<string>()
                 ).IsRequired(false).Metadata.SetValueComparer(new ValueComparer<List<string>>(
                 (c1, c2) => c1.SequenceEqual(c2),
                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                 c => c.ToList()
                 ));
          

                modelBuilder.Entity<UserAllergy>(entity =>
            {
                entity.HasKey(entity => entity.Id);
                entity.HasIndex(ua => new { ua.UserId, ua.AllergyId }).IsUnique();


            });
                modelBuilder.Entity<UserInjury>(entity =>
                {
                    entity.HasKey(entity => entity.Id);
                    entity.HasIndex(ua => new { ua.UserId, ua.injuryId }).IsUnique();
                    entity.HasOne(ua => ua.injury).WithMany(ua => ua.UserInjuries).HasForeignKey(ua => ua.injuryId);

                });
                modelBuilder.Entity<WorkOutPlan>(entity =>
                {
                    entity.Property(u => u.DifficultyLevel).HasConversion<string>();
                    entity.HasMany(w => w.workOutExercises).WithOne(w => w.workOutPlan).HasForeignKey(w => w.WorkoOutId);
                });
                modelBuilder.Entity<Injury>(entity =>
                {
                    entity.Property(e => e.SuitableExercises).HasConversion(
                        v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                        v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions)null) ?? new List<string>()
                       ).IsRequired(false).Metadata.SetValueComparer(new ValueComparer<List<string>>(
                           (c1, c2) => c1.SequenceEqual(c2),
                           c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                           c => c.ToList()
                       ));

                    entity.Property(e => e.SuitableEquipment).HasConversion(
                    v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                    v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions)null) ?? new List<string>()
                    ).IsRequired(false).Metadata.SetValueComparer(new ValueComparer<List<string>>(
                    (c1, c2) => c1.SequenceEqual(c2),
                    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                    c => c.ToList()
                    ));

                    entity.Property(e => e.ContraindicatedExercises).HasConversion(
                    v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                    v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions)null) ?? new List<string>()
                    ).IsRequired(false).Metadata.SetValueComparer(new ValueComparer<List<string>>(
                    (c1, c2) => c1.SequenceEqual(c2),
                    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                    c => c.ToList()
                    ));

                });
            });
            modelBuilder.Entity<WorkOutExercises>(entity =>
            {
                entity.HasKey(e => e.Id);

            });
            modelBuilder.Entity<UserAllergy>(entity =>
            {
                entity.HasKey(entity => entity.Id);
                entity.HasIndex(ua => new { ua.UserId, ua.AllergyId }).IsUnique();
                entity.HasOne(ua => ua.allergy).WithMany(ua => ua.userAllergies).HasForeignKey(ua => ua.AllergyId);
            });
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);


        }
        public DbSet<LogFood> LogFood { get; set; }
        public DbSet<DailyIntake> dailyIntakes { get; set; }

        public DbSet<FoodItem> Food { get; set; }
        public DbSet<Exercise> Exercise { get; set; }
        public DbSet<Injury> Injury { get; set; }
        public DbSet<Allergy> Allergy { get; set; }
        public DbSet<WorkOutPlan> WorkOutPlans { get; set; }
        public DbSet<UserGoal> userGoals { get; set; }
        public DbSet<GoalTempelate> GoalTempelate { get; set; }

        public DbSet<UserMetrics> userMetrics { get; set; }
        public DbSet<UserInjury> userInjuries { get; set; }
        public DbSet<UserAllergy> userAllergies { get; set; }
        public DbSet<NutritionPlan> nutritionPlans { get; set; }
        public DbSet<WorkOutExercises> workOutExercises { get; set; }
        public DbSet<ExerciseLog> exerciseLogs { get; set; }
        public DbSet<Exercise_Feedback> exercise_Feedbacks { get; set; }




    }
}
