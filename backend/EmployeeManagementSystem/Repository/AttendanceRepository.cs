using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Helpers;
using EmployeeManagementSystem.Interfaces;
using EmployeeManagementSystem.Model;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementSystem.Repository
{
    public class AttendanceRepository : Repository<Attendance>, IAttendanceRepository
    {
        private readonly ApplicationDBContext _context;

        public AttendanceRepository(ApplicationDBContext context) : base(context)
        {
            _context = context;
        }
        public async Task<PagedResult<Attendance>> GetAttendanceHistoryAsync(SearchOptions options)
        {
            var query = _context.Attendances.AsNoTracking().AsQueryable();

            // Filter by employee
            if (options.EmployeeId.HasValue)
                query = query.Where(x => x.EmployeeId == options.EmployeeId.Value);

            // Get total count before pagination
            var totalCount = await query.CountAsync();

            // Apply pagination if given
            if (options.PageIndex.HasValue)
            {
                query = query
                    .Skip(options.PageIndex.Value * options.PageSize)
                    .Take(options.PageSize);
            }

            var data = await query.ToListAsync();

            return new PagedResult<Attendance>
            {
                TotalCount = totalCount,
                Data = data
            };
        }
    }
}
