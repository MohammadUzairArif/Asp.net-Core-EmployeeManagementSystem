using EmployeeManagementSystem.Interfaces;
using EmployeeManagementSystem.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IRepository<Department> departmentRepository;

        public DepartmentController(IRepository<Department> departmentRepository)
        {
            this.departmentRepository = departmentRepository;
        }

        [HttpPost]
        public async Task<IActionResult> addDepartment([FromBody] Department model)
        {
            await departmentRepository.AddAsync(model);
            await departmentRepository.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> updateDepartment(int id,[FromBody] Department model)
        {
            var department = await departmentRepository.GetByIdAsync(id);
            department.Name = model.Name;
            departmentRepository.Update(department);
            
            await departmentRepository.SaveChangesAsync();
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> getAllDepartment()
        {
            var list = await departmentRepository.GetAllAsync();

            return Ok(list);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            await departmentRepository.DeleteAsync(id);


            await departmentRepository.SaveChangesAsync();
            return Ok();
        }

    }
}
