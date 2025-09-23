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
                    await userManager.AddToRoleAsync(appUser, "Admin"); // "User" role assign kiya

                    //if (roleResult.Succeeded)
                    //{
                    var token = await tokenService.CreateToken(appUser); // JWT token banaya
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
                    return StatusCode(500, new { message = createdUser.Errors }); // User create nahi ho saka
                }
            }


            catch (Exception e)
            {
                return StatusCode(500, new { message = e.Message }); // Unexpected error handle
            }


        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]  AuthDto loginDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);


                var user = await userManager.FindByEmailAsync(loginDto.Email);


                if (user == null)
                    return Unauthorized(new { message = "Invalid email or password" }); // User exist nahi karta


                var result = await signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);



                if (!result.Succeeded)
                {
                    return Unauthorized(new { message = "Invalid email or password" }); // Password match nahi hua
                }

                var token = await tokenService.CreateToken(user);

                var userDto = user.ToLoginUserDto(token);
                return Ok(userDto);
            }
            catch (Exception e)
            {
                return StatusCode(500, new { message = e.Message });
            }

        }
    }
}

