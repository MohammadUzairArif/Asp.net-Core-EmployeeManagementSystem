using Microsoft.AspNetCore.Identity;

namespace EmployeeManagementSystem.Seed
{
    public class RoleSeeder
    {
        private static readonly string[] Roles = new[] { "Admin", "Manager", "Employee" };

        public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            foreach (var role in Roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }

    }
}
