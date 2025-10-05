using EmployeeManagementSystem.Helpers;
using EmployeeManagementSystem.Model;

namespace EmployeeManagementSystem.Interfaces
{
    public interface IDepartmentRepository : IRepository<Department>
    {
        // Repository-level method jo search + paging handle karega
        Task<PagedResult<Department>> GetAllAsync(SearchOptions options);
    }
}
