using EmployeeManagementSystem.Model;

namespace EmployeeManagementSystem.Interfaces
{
    public interface ITokenService
    {
       string CreateToken(User user);
    }
}
