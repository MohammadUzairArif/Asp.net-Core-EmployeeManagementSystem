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
        [HttpPost]
        public async Task<IActionResult> UpdateProfile([FromBody] ProfileDto model)
        {
            // Get current logged-in user
            var userId = model.UserId; // Assuming UserId is passed in the DTO for simplicity
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User not found.");

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound(new { message = "User does not exist." });

            // 🔹 Update User (AspNetUsers)
            user.UpdateUserFromDto(model);
            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
                return BadRequest(new { message = result.Errors.First().Description });

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
                    return BadRequest(new { message = pwdResult.Errors.First().Description });
            }


            return Ok(new { message = "Profile updated successfully." });
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProfile(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound(new { message = "User does not exist." });

            var employee = await _employeeRepo.FindAsync(e => e.UserId == id);
            if (employee == null)
                return NotFound(new { message = "Employee does not exist." });


            var profile = (user, employee).ToProfileResponseDto();

            return Ok(profile);
        }
    }
}
