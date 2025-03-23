
using FitGuide.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitGuide.Data.Configurations
{
   

    public class WorkoutPlanExerciseConfig : IEntityTypeConfiguration<WorkoutPlanExercise>
    {
        public void Configure(EntityTypeBuilder<WorkoutPlanExercise> builder)
        {
            builder.HasKey(wpe => new { wpe.WorkoutPlanId, wpe.ExerciseId });

            builder.HasOne(wpe => wpe.WorkoutPlan)
                .WithMany(wp => wp.Exercises)
                .HasForeignKey(wpe => wpe.WorkoutPlanId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(wpe => wpe.Exercise)
                .WithMany(e => e.WorkoutPlans)
                .HasForeignKey(wpe => wpe.ExerciseId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

}
