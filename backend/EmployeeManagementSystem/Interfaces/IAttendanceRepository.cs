using EmployeeManagementSystem.Helpers;
using EmployeeManagementSystem.Model;

namespace EmployeeManagementSystem.Interfaces
{
    public interface IAttendanceRepository : IRepository<Attendance>
    {
        Task<PagedResult<Attendance>> GetAttendanceHistoryAsync(SearchOptions options);
    }
}
