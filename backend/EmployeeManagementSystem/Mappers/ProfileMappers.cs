using EmployeeManagementSystem.Dto;
using EmployeeManagementSystem.Model;

namespace EmployeeManagementSystem.Mappers
{
    public static class ProfileMappers
    {
        public static void UpdateUserFromDto(this User user, ProfileDto dto)
        {
            
            user.ProfileImage = dto.ProfileImage;
            // Password yahan nahi update karenge, woh UserManager karega
        }

        public static void UpdateEmployeeFromDto(this Employee employee, ProfileDto dto)
        {
            employee.Name = dto.Name;
            
            employee.Phone = dto.Phone;
        }

        public static ProfileResponseDto ToProfileResponseDto(this (User user, Employee employee) data)
        {
            return new ProfileResponseDto
            {
                Name = data.employee?.Name,
                Phone = data.employee?.Phone,
                ProfileImage = data.user?.ProfileImage
            };
        }
    }
}
