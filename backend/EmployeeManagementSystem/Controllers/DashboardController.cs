using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Dto;
using EmployeeManagementSystem.Interfaces;
using EmployeeManagementSystem.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IRepository<Employee> _empRepo;
        private readonly IRepository<Department> _depRepo;
        private readonly ApplicationDBContext _context;
        public DashboardController(IRepository<Employee> empRepo, IRepository<Department> depRepo, ApplicationDBContext context)
        {
            _empRepo = empRepo;
            _depRepo = depRepo;
            _context = context;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> TotalSalary()
        {
            var empList = await _empRepo.GetAllAsync();
            var depList = await _depRepo.GetAllAsync();
            var totalSalary = empList.Sum(x => x.Salary ?? 0);
            var employeeCount = empList.Count;
            var depCount = depList.Count;
            return Ok(new
            {
                TotalSalary = totalSalary,
                EmployeeCount = employeeCount,
                DepartmentCount = depCount
            });
        }

        [HttpGet("department-data")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetDepartmentData()
        {
            // Only showing departments that currently have employees
            //yeh hm db level pr kr rhy hn taake zyada efficient ho
            // yeh stored procedure nhi hy complex project my stored procedure use kr skty hen
            var result = await _context.Employees
                .GroupBy(e => e.DepartmentId)
                .Select(g => new DepartmentDataDto
                {
                    Name = g.First().Department.Name,
                    EmployeeCount = g.Count()
                })
                .OrderByDescending(x => x.EmployeeCount)
                .ToListAsync();

            return Ok(result);
        }
    }
}
