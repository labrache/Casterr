using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Casterr.Data
{
    public static class ApplicationDataInitialiser
    {
        public static void SeedData(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            SeedRoles(roleManager);
            SeedUsers(userManager);
        }

        public static void SeedUsers(UserManager<AppUser> userManager)
        {
            if (userManager.FindByNameAsync("admin").Result == null)
            {
                var user = new AppUser
                {
                    UserName = "admin",
                    Email = "admin@localhost",
                    NormalizedUserName = "ADMIN",
                    NormalizedEmail = "ADMIN@LOCALHOST",
                    EmailConfirmed = true
                };

                var password = "casterr";

                var result = userManager.CreateAsync(user, password).Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Admin").Wait();
                }
            }
        }

        public static void SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.RoleExistsAsync("Admin").Result)
            {
                var role = new IdentityRole
                {
                    Name = "Admin"
                };
                roleManager.CreateAsync(role).Wait();
            }
        }
    }
}
