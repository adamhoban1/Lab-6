using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Student_Class_Library;


namespace Student_MVC_App.data
{
    public static class IdentitySeed
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<Appusers>>();

            string[] roles = new[] { "Admin", "Manager", "User" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }

            // create admin
            var adminEmail = "admin@local";
            var admin = await userManager.FindByEmailAsync(adminEmail);
            if (admin == null)
            {
                admin = new Appusers
                {
                    UserName = "admin",
                    Email = adminEmail,
                    Name = "Site Admin",
                    Age = 30
                };
                var res = await userManager.CreateAsync(admin, "Admin123!");
                if (res.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "Admin");
                }
                else
                {
                    throw new Exception(string.Join("; ", res.Errors));
                }
            }
            else
            {
                if (!await userManager.IsInRoleAsync(admin, "Admin"))
                    await userManager.AddToRoleAsync(admin, "Admin");
            }
        }
    }
}