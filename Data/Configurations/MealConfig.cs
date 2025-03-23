using FitGuide.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace FitGuide.Data.Configurations
{
    

    public class MealConfig : IEntityTypeConfiguration<Meal>
    {
        public void Configure(EntityTypeBuilder<Meal> builder)
        {
            builder.Property(m => m.MealName)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasOne(m => m.NutritionPlan)
                .WithMany(np => np.Meals)
                .HasForeignKey(m => m.NutritionPlanId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

}
