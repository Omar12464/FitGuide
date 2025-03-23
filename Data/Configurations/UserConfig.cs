using FitGuide.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitGuide.Data.Configurations
{
    

    public class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(u => u.Password)
                .IsRequired();

            builder.Property(u => u.BirthDate)
                .IsRequired();

            builder.Property(u => u.Gender)
                .HasMaxLength(10);

        }
    }

}
