using Core.Identity.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Identity
{
    public class AppIdentityDbContext:IdentityDbContext<User>
    {
        private readonly DbContextOptions<AppIdentityDbContext> _options;

        public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options):base(options)
        {
            _options = options;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>(e =>
            {
            //e.HasKey(u => u.Id);
            e.HasIndex(u => u.UserName).IsUnique();
            e.HasIndex(u => u.Email).IsUnique();
            e.HasMany(ua => ua.userAllergies).WithOne(u => u.user).HasForeignKey(u => u.UserId);
            e.HasMany(ua => ua.userInjuries).WithOne(u => u.user).HasForeignKey(u => u.UserId);



                e.Property(u=>u.UserName).IsRequired().HasMaxLength(50);
                e.Property(u => u.Email).IsRequired().HasMaxLength(100);
                e.Property(u=>u.PasswordHash).IsRequired().HasMaxLength(60);
                e.Property(u => u.TwoFactorEnabled).IsRequired();
                e.Property(u => u.CreatedAt).HasDefaultValueSql("GETDATE()");


            });
            builder.Entity<UserAllergy>(e =>
            {
                e.HasKey(ua => new { ua.UserId, ua.AllergyId });
                e.HasOne(ua => ua.user).WithMany(ua => ua.userAllergies).HasForeignKey(u => u.UserId);

            });
            builder.Entity<UserInjury>(e =>
            {
                e.HasKey(ua => new { ua.UserId, ua.InjuryId });
                e.HasOne(ua => ua.user).WithMany(ua => ua.userInjuries).HasForeignKey(u => u.UserId);

            });

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

    }
}
