using EmployeeManagementSystem.Dto;
using EmployeeManagementSystem.Model;

namespace EmployeeManagementSystem.Mappers
{
    public static class AuthMapper
    {
        public static User ToAppUser(this AuthDto dto)
        {
            return new User
            {

                Email = dto.Email,
                // dont use Password here, it will be set by UserManager in controller because it hased and salted automatically
                UserName = dto.Email
            };
        }

        public static RegisterTokenDto ToNewUserDto(this User user, string token,string role)
        {
            return new RegisterTokenDto
            {
               
                Email = user.Email,
                Token = token,
                Role = role
            };
        }

        public static LoginTokenDto ToLoginUserDto (this User user, string token,string role)
        {
            return new LoginTokenDto
            {
                Id = user.Id,
                Email = user.Email,
                Token = token,
                Role = role
            };
        }
    }
}
