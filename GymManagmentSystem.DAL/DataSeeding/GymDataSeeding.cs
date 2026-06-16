using GymManagmentSystem.DAL.dbcontext;
using GymManagmentSystem.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GymManagmentSystem.DAL.DataSeeding
{
    public static class GymDataSeeding
    {
        public static async Task SeedAsync(GymDbContext dbContext,string seedFilePath,ILogger logger,CancellationToken ct=default)
        {
            try
            {

                if (!await dbContext.Plans.AnyAsync(ct))
                {
                    var Plans = LoadDataFromJasonFile<Plan>("plans.json", seedFilePath);
                    if (Plans.Count > 0)
                    {
                        dbContext.Plans.AddRange(Plans);
                        logger.LogInformation("Seeded {Count} plans", Plans.Count);

                    }
                }
                if (dbContext.ChangeTracker.HasChanges())
                {
                    await dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while seeding the database.");
                throw;

            }

            
        }
        private static List<T>LoadDataFromJasonFile<T>(string FileName,string FolderPath)
        {
            var filePath = Path.Combine(FolderPath, FileName);
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"The file '{filePath}' was not found.");
            }
            var Data = File.ReadAllText(filePath);
            var Options=new JsonSerializerOptions {PropertyNameCaseInsensitive=true};
            Options.Converters.Add(new JsonStringEnumConverter());
            return JsonSerializer.Deserialize<List<T>>(Data, Options) ?? [];


          
        }


    }
}
