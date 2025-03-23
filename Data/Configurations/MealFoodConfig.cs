

using FitGuide.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;



namespace FitGuide.Data.Configurations
{

    public class MealFoodConfig : IEntityTypeConfiguration<MealFood>
    {
        public void Configure(EntityTypeBuilder<MealFood> builder)
        {
            builder.HasKey(mf => new { mf.MealId, mf.FoodId });

            builder.HasOne(mf => mf.Meal)
                .WithMany(m => m.Foods)
                .HasForeignKey(mf => mf.MealId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(mf => mf.Food)
                .WithMany(f => f.Meals)
                .HasForeignKey(mf => mf.FoodId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

}
