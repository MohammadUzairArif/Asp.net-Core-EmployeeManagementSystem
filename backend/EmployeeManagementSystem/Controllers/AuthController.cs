using EmployeeManagementSystem.Dto;
using EmployeeManagementSystem.Interfaces;
using EmployeeManagementSystem.Mappers;
using EmployeeManagementSystem.Model;
using EmployeeManagementSystem.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly ITokenService tokenService;
        

        public AuthController(UserManager<User> userManager, SignInManager<User> signInManager, ITokenService tokenService)
        {

            this.userManager = userManager;
            this.signInManager = signInManager;
            this.tokenService = tokenService;
        }

        

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

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]  AuthDto loginDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState); // Input validation fail ho gayi

                // Email ke zariye user find karo
                var user = await userManager.FindByEmailAsync(loginDto.Email);

                if (user == null)
                    return Unauthorized("Invalid email or password"); // User exist nahi karta

                // Password check karo Identity system se
                var result = await signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);


                if (!result.Succeeded)
                {
                    return Unauthorized("Invalid email or password"); // Password match nahi hua
                }

                var token = tokenService.CreateToken(user); // Token generate karo

                var userDto = user.ToLoginUserDto(token);    // DTO banayo login response ke liye
                return Ok(userDto);                          // 200 OK with token
            }
            catch (Exception e)
            {
                return StatusCode(500, e); // Koi bhi runtime error
            }
        }
    }
}