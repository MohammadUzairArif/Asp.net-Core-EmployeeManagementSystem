using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Interfaces;
using EmployeeManagementSystem.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        
        private readonly IRepository<Employee> employeeRepository;

        public EmployeeController(IRepository<Employee> employeeRepository) {
            this.employeeRepository = employeeRepository;
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllEmployees()
        {
            // Logic to retrieve all employees
            return Ok(await employeeRepository.GetAllAsync());
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddEmployee([FromBody] Employee employeeModel)
        {
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
