using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Repository
{
    public class FitGuideContextFactory : IDesignTimeDbContextFactory<FitGuideContext>
    {
        public FitGuideContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var builder = new DbContextOptionsBuilder<FitGuideContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnection") ?? 
                "Server=.;Database=FitGuide;Trusted_Connection=True;TrustServerCertificate=True";

            builder.UseSqlServer(connectionString);

            return new FitGuideContext(builder.Options);
        }
    }
}
