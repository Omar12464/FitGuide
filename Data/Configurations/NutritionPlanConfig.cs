
using FitGuide.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitGuide.Data.Configurations
{
   

    public class NutritionPlanConfig : IEntityTypeConfiguration<NutritionPlan>
    {
        public void Configure(EntityTypeBuilder<NutritionPlan> builder)
        {
            builder.Property(np => np.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.HasOne(np => np.User)
                .WithMany(u => u.NutritionPlans)
                .HasForeignKey(np => np.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

}
