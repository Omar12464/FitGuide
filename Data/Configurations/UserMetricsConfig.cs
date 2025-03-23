namespace FitGuide.Data.Configurations
{
    using FitGuide.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class UserMetricsConfig : IEntityTypeConfiguration<UserMetrics>
    {
        public void Configure(EntityTypeBuilder<UserMetrics> builder)
        {
            builder.Property(um => um.BMI).IsRequired();
            builder.Property(um => um.BodyFat).IsRequired();
            builder.Property(um => um.MuscleMass).IsRequired();
            builder.Property(um => um.WaterMass).IsRequired();

            builder.HasOne(um => um.User)
                .WithMany(u => u.Metrics)
                .HasForeignKey(um => um.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

}
