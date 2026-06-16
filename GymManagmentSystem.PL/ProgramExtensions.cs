using GymManagmentSystem.DAL.DataSeeding;
using GymManagmentSystem.DAL.dbcontext;
using Microsoft.EntityFrameworkCore;

namespace GymManagmentSystem.PL
{
    public static class ProgramExtensions
    {
        public static async Task MigrateAndSeedDatabaseAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<GymDbContext>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
            var pendingMigrations = await dbContext.Database.GetPendingMigrationsAsync();

            if (pendingMigrations.Any())
            { 
                logger.LogInformation("Applying {Count} pending migrations...", pendingMigrations.Count());
                await dbContext.Database.MigrateAsync();
               
            }
            var seedFolderPath = Path.Combine(app.Environment.ContentRootPath, "wwwroot", "Files");
            await GymDataSeeding.SeedAsync(dbContext, seedFolderPath, logger);

        }
    }
}
