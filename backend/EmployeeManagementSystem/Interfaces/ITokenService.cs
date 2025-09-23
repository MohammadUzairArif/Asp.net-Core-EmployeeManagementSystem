using EmployeeManagementSystem.Model;

namespace EmployeeManagementSystem.Interfaces
{
    public interface ITokenService
    {
       Task<string> CreateToken(User user);
    }
}
