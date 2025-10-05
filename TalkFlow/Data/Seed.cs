using TalkFlow.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace TalkFlow.Data
{
    public class Seed
    {
        public static async Task SeedUsers(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            if (await userManager.Users.AnyAsync()) return;

            var roles = new List<AppRole> {
                new AppRole { Id = Guid.NewGuid(), Name = "Admin" },
                new AppRole { Id = Guid.NewGuid(), Name = "Owner" },
                new AppRole { Id = Guid.NewGuid(), Name = "Guest" },
                new AppRole { Id = Guid.NewGuid(), Name = "Host" },
                new AppRole { Id = Guid.NewGuid(), Name = "Member" }
            };

            if (!await roleManager.Roles.AnyAsync())
            {
                foreach (var role in roles)
                {
                    await roleManager.CreateAsync(role);
                }
            }

            var admin = new AppUser { UserName = "admin", DisplayName = "Administrator" };
            await userManager.CreateAsync(admin, "admin@123");
            await userManager.AddToRolesAsync(admin, new[] { "Admin", "Owner", "Guest" });
        }
    }
}


