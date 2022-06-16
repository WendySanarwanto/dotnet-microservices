using Microsoft.EntityFrameworkCore;
using PlatformService.Models;

namespace PlatformService.Data {
    public static class PrepareDb {
        public static void PrepPopulation(WebApplication app) {
            using(var serviceScope = app.Services.CreateScope()) {
                SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>(), 
                        app.Environment.IsProduction());
            }
        }

        private static void SeedData(AppDbContext dbContext, bool isProduction = false) {
            if (!isProduction) {
                if (!dbContext.Platforms.Any()) {
                    Console.WriteLine("--> Seeding data on In-Memory Database ...");

                    dbContext.Platforms.AddRange(
                        new Platform { Name="Dot Net", Publisher="Microsoft", Cost="Free"},
                        new Platform { Name="SQL Server Express", Publisher="Microsoft", Cost="Free"},
                        new Platform { Name="Kubernetes", Publisher="Cloud Native Computing Foundation", Cost="Free"}
                    );

                    dbContext.SaveChanges();
                } else {
                    Console.WriteLine("--> We already have data.");
                }
            } else {
                Console.WriteLine("---> Attempting to apply migrations to SQL Server Database ...");                
                try {
                    dbContext.Database.Migrate();
                } catch(Exception exception) {
                    Console.WriteLine($"---> Cannot run migrations: {exception.Message}. ");
                }
            }
        }
    }
}