using EmployeeManagementSystem.Dto;
using EmployeeManagementSystem.Interfaces;
using EmployeeManagementSystem.Mappers;
using EmployeeManagementSystem.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly UserManager<User> userManager;
        private readonly ITokenService tokenService;
        private readonly SignInManager<User> signInManager;

        public AuthController(UserManager<User> userManager, SignInManager<User> signInManager, ITokenService tokenService)
        {

            this.userManager = userManager;
            this.SignInManager = signInManager;
            this.tokenService = tokenService;
        }

        public SignInManager<User> SignInManager { get; }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] AuthDto authDto)
        {
            try
            {

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var appUser = authDto.ToAppUser();
                var createdUser = await userManager.CreateAsync(appUser, authDto.Password); // User create kiya



                if (createdUser.Succeeded)
                {
                    //var roleResult = await _userManager.AddToRoleAsync(appUser, "User"); // "User" role assign kiya

                    //if (roleResult.Succeeded)
                    //{
                    var token = tokenService.CreateToken(appUser); // JWT token banaya
                    var newUserDto = appUser.ToNewUserDto(token);   // DTO banaya response ke liye
                    return Ok(newUserDto);                          // 200 OK response with token
                                                                    //}
                                                                    //else
                                                                    //{
                                                                    //    return StatusCode(500, roleResult.Errors); // Role assign mein error
                                                                    //}
                }
                else
                {
                    return StatusCode(500, createdUser.Errors); // User create nahi ho saka
                }
            }

            catch (Exception e)
            {
                return StatusCode(500, e); // Unexpected error handle
            }

        }
    }
}