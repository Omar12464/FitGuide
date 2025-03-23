
using FitGuide.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace FitGuide.Data.Configurations

{


    public class UserGoalConfig : IEntityTypeConfiguration<UserGoal>
    {
        public void Configure(EntityTypeBuilder<UserGoal> builder)
        {
            builder.Property(ug => ug.Weight).IsRequired();
            builder.Property(ug => ug.BMI).IsRequired();
            builder.Property(ug => ug.MuscleMass).IsRequired();
            builder.Property(ug => ug.StartDate).IsRequired();
            builder.Property(ug => ug.IsAchieved).HasDefaultValue(false);

            builder.HasOne(ug => ug.User)
                .WithMany(u => u.Goals)
                .HasForeignKey(ug => ug.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

}
