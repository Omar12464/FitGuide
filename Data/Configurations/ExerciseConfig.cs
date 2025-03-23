
using FitGuide.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace FitGuide.Data.Configurations
{
   

    public class ExerciseConfig : IEntityTypeConfiguration<Exercise>
    {
        public void Configure(EntityTypeBuilder<Exercise> builder)
        {
            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(e => e.Description)
                .HasMaxLength(500);

            builder.Property(e => e.TargetMuscle)
                .HasMaxLength(50);

            builder.Property(e => e.TypeOfMachine)
                .HasMaxLength(50);
        }
    }

}
