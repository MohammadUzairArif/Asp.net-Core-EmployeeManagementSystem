using EmployeeManagementSystem.Dto;
using EmployeeManagementSystem.Interfaces;
using EmployeeManagementSystem.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IRepository<User> userRepo;

        public AuthController(IRepository<User> userRepo)
        {
            this.userRepo = userRepo;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AuthDto model)
        {
            // Logic to authenticate user and generate JWT token
            var getUsers = await userRepo.GetAllAsync();
            var user = getUsers.FirstOrDefault(u => u.Email == model.Email);
            if (user == null || user.Password != model.Password)
            {
                return Unauthorized("Invalid email or password");
            }
            return Ok();
        }
    }
}