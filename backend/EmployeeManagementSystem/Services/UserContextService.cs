using EmployeeManagementSystem.Interfaces;
using EmployeeManagementSystem.Model;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace EmployeeManagementSystem.Services
{
    public class UserContextService : IUserContextService
    {
        private readonly UserManager<User> userManager;
        private readonly IEmployeeRepository employeeRepo;

        public UserContextService(UserManager<User> userManager, IEmployeeRepository employeeRepo)
        {
            this.userManager = userManager;
            this.employeeRepo = employeeRepo;
        }

        public async Task<int?> GetEmployeeIdFromClaimsAsync(ClaimsPrincipal principal)
        {
            // 1️ Extract email from JWT claims
            var email = principal.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(email))
                return null; // no email in token

            // 2️ Find the user in Identity
            var appUser = await userManager.FindByEmailAsync(email);
            if (appUser == null)
                return null; // no user found

            // 3️ Find the employee record linked to the user
            var employee = await employeeRepo.FindAsync(e => e.UserId == appUser.Id);
            if (employee == null)
                return null;

            // 4️ Return the employee's ID
            return employee.Id;
        }

        public async Task<string?> GetUserIdFromClaimsAsync(ClaimsPrincipal principal)
        {
            // 1️ Extract email from JWT claims
            var email = principal.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(email))
                return null;

            // 2️ Find the user in Identity
            var appUser = await userManager.FindByEmailAsync(email);
            if (appUser == null)
                return null;

            // 3️ Return the user's Id
            return appUser.Id;
        }

        public bool IsAdmin(ClaimsPrincipal principal)
        {
            var role = principal.FindFirstValue(ClaimTypes.Role);
            return role == "Admin";
        }
    }
}
