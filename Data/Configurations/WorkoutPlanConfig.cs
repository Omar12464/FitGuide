using FitGuide.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace FitGuide.Data.Configurations
{
    

    public class WorkoutPlanConfig : IEntityTypeConfiguration<WorkoutPlan>
    {
        public void Configure(EntityTypeBuilder<WorkoutPlan> builder)
        {
            builder.Property(wp => wp.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(wp => wp.Description)
                .HasMaxLength(500);

            builder.HasOne(wp => wp.User)
                .WithMany(u => u.WorkoutPlans)
                .HasForeignKey(wp => wp.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

}
