using Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class FitGuideContext:DbContext
    {
        public FitGuideContext(DbContextOptions<FitGuideContext> options):base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Food>(entity =>
            {
                entity.HasKey(f => f.Id);

                entity.Property(f => f.Name).IsRequired().HasMaxLength(200);

                entity.Property(f => f.Calories).HasColumnType("decimal(10,2)");
                entity.Property(f => f.Protein).HasColumnType("decimal(10,2)");
                entity.Property(f => f.Carbs).HasColumnType("decimal(10,2)");
                entity.Property(f => f.Fats).HasColumnType("decimal(10,2)");

            });
            modelBuilder.Entity<Exercise>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e=>e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.GifBytes).HasColumnType("varbinary(max)");
                entity.Property(e=>e.GifPath).HasMaxLength(50);
            });
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            
        }
        public DbSet<Food> Food { get; set; }
        public DbSet<Exercise> Exerciseee { get; set; }
        public DbSet<Injury> Injury { get; set; }
        public DbSet<Allergy> Allergy { get; set; }
        public DbSet<WorkOutPlan> WorkOutPlans { get; set; }

    }
}
