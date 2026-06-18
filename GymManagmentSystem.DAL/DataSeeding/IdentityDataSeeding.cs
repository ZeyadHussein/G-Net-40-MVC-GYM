using GymManagmentSystem.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentSystem.DAL.DataSeeding
{
    public static class IdentityDataSeeding
    {
        public static async Task SeedAsync( RoleManager<IdentityRole>roleManager,UserManager<ApplicationUser> userManager, ILogger logger,CancellationToken ct=default)
        {
            try
            {
                bool HasUsers= userManager.Users.Any();
                bool HasRoles= roleManager.Roles.Any();
                if(HasRoles && HasUsers) return;
                if (!HasRoles)
                {
                    var Roles = new List<IdentityRole>
                    {
                        new IdentityRole(){Name="SuperAdmin"},
                        new IdentityRole(){Name="Admin"}
                    };
                    foreach (var roleName in Roles.Select(r => r.Name))
                    {
                        if (!await roleManager.RoleExistsAsync(roleName))
                        {
                            var result = await roleManager.CreateAsync(new IdentityRole(roleName));

                            if (!result.Succeeded)
                            {
                                logger.LogError(
                                    $"Failed to create role {roleName}: {string.Join(", ", result.Errors.Select(e => e.Description))}"
                                );
                            }
                        }
                    }
                }
                if (!HasUsers)
                {
                    var MainAdminUser = new ApplicationUser()
                    {
                        FirstName = "Zeyad",
                        LastName = "Hussein",
                        UserName = "ZeyadHussein",
                        Email = "zeyad.hussein@gmail.com",
                        PhoneNumber = "01001793955"


                    };
                    var result = await userManager.CreateAsync( MainAdminUser, "P@ssw0rd");

                    if (!result.Succeeded)
                    {
                        logger.LogError(
                            "Failed to create SuperAdmin: {Errors}",
                            string.Join(", ",
                            result.Errors.Select(e => e.Description)));

                        return;
                    }

                    await userManager.AddToRoleAsync(
                        MainAdminUser,
                        "SuperAdmin");

                    var Admin01 = new ApplicationUser()
                    {
                        FirstName = "Ahmed",
                        LastName = "Ali",
                        UserName = "AhmedAli",
                        Email = "ahmed.ali@gmail.com",
                        PhoneNumber = "01001793956"


                    };
                    var CreateResult = await userManager.CreateAsync(Admin01, "P@ssw0rd");
                    if (!CreateResult.Succeeded)
                    {
                        logger.LogError("Failed to create seed SuperAdmin:[Errors]", string.Join(", ", CreateResult.Errors.Select(e => e.Description)));
                        return;
                    }
                    logger.LogInformation($"Seeded SuperAdmin user {Admin01.Email}.");

                }
                return;

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred during identity data seeding.");
                throw;
            }
        }
    }
}
