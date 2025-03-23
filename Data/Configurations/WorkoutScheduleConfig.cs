using FitGuide.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitGuide.Data.Configurations
{
    

    public class WorkoutScheduleConfig : IEntityTypeConfiguration<WorkoutSchedule>
    {
        public void Configure(EntityTypeBuilder<WorkoutSchedule> builder)
        {
            builder.Property(ws => ws.WorkoutDate)
                .IsRequired();

            builder.HasOne(ws => ws.WorkoutPlan)
                .WithMany(wp => wp.Schedules)
                .HasForeignKey(ws => ws.WorkoutPlanId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

}
