using EmployeeManagementSystem.Helpers;
using EmployeeManagementSystem.Model;

namespace EmployeeManagementSystem.Interfaces
{
    public interface IEmployeeRepository: IRepository<Employee>
    {
        // Repository-level method jo search + paging handle karega
        Task<List<Employee>> GetAllAsync(SearchOptions options);
    }
}
