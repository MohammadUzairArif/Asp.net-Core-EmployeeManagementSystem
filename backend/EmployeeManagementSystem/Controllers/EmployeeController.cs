using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Helpers;
using EmployeeManagementSystem.Interfaces;
using EmployeeManagementSystem.Model;
using EmployeeManagementSystem.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {

        private readonly IEmployeeRepository employeeRepository;
        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public EmployeeController(IEmployeeRepository employeeRepository, UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager) {
            this.employeeRepository = employeeRepository;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllEmployees([FromQuery] SearchOptions options)
        {
            var employees = await employeeRepository.GetAllAsync(options);
            return Ok(employees);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddEmployee([FromBody] Employee employeeModel)
        {
            // 1. Create Identity user
            var user = new User
            {
                UserName = employeeModel.Email,   // ya model.Name agar username alag lena ho
                Email = employeeModel.Email
            };

            var result = await userManager.CreateAsync(user, "Employee@123"); // fixed password

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            // 2. Ensure Employee role exists
            if (!await roleManager.RoleExistsAsync("Employee"))
            {
                await roleManager.CreateAsync(new IdentityRole("Employee"));
            }

            // 3. Assign Employee role
            await userManager.AddToRoleAsync(user, "Employee");

            // 4. Save employee in custom Employee table with UserId
            employeeModel.UserId = user.Id;   //  Identity user ka Id store
            // Logic to add a new employee
            await employeeRepository.AddAsync(employeeModel);
            await employeeRepository.SaveChangesAsync();
            return Ok();
        }
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateEmployee(int id,[FromBody] Employee employeeModel)
        {
            // Logic to update a new employee
            var employee = await employeeRepository.GetByIdAsync(id);
            employee.Name = employeeModel.Name;
            employee.Email = employeeModel.Email;
            employee.Phone = employeeModel.Phone;
            employee.DepartmentId = employeeModel.DepartmentId;
            employee.JobTitle = employeeModel.JobTitle;
            employee.LastWorkingDate = employeeModel.LastWorkingDate;

            employeeRepository.Update(employee);
            await employeeRepository.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            // Logic to delete a new employee
            await employeeRepository.DeleteAsync(id);
            await employeeRepository.SaveChangesAsync();
            return Ok();
        }
    }
}
