using EmployeeManagementSystem.Dto;
using EmployeeManagementSystem.Interfaces;
using EmployeeManagementSystem.Mappers;
using EmployeeManagementSystem.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IRepository<Employee> _employeeRepo;

        public ProfileController(UserManager<User> userManager, IRepository<Employee> employeeRepo)
        {
            _userManager = userManager;
            _employeeRepo = employeeRepo;
        }

        [Authorize]
        [HttpPost("Profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] ProfileDto model)
        {
            // Get current logged-in user
            var userId = model.UserId; // Assuming UserId is passed in the DTO for simplicity
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User not found.");

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound("User does not exist.");

            // 🔹 Update User (AspNetUsers)
            user.UpdateUserFromDto(model);
            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            // 🔹 Update Employee
            var employee = await _employeeRepo.FindAsync(e => e.UserId == userId);
            if (employee != null)
            {
                employee.UpdateEmployeeFromDto(model);
                _employeeRepo.Update(employee);
                await _employeeRepo.SaveChangesAsync();
            }

            // 🔹 Update Password (if provided)
            if (!string.IsNullOrEmpty(model.OldPassword) && !string.IsNullOrEmpty(model.NewPassword))
            {
                var pwdResult = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                if (!pwdResult.Succeeded)
                    return BadRequest(pwdResult.Errors);
            }


            return Ok("Profile updated successfully.");
        }
    }
}
