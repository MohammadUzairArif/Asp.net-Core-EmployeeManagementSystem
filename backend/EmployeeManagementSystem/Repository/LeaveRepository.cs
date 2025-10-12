using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Helpers;
using EmployeeManagementSystem.Interfaces;
using EmployeeManagementSystem.Model;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementSystem.Repository
{
    public class LeaveRepository : Repository<Leave>, ILeaveRepository
    {

        private readonly ApplicationDBContext _context;


        public LeaveRepository(ApplicationDBContext context) : base(context)
        {
            _context = context;

        }
        public async Task<PagedResult<Leave>> GetAllAsync(SearchOptions options)
        {
            var query = _context.Leaves.AsQueryable();
            // Get total count before pagination
            var totalCount = await query.CountAsync();

            // Apply pagination
            if (options.PageIndex.HasValue)
            {
                query = query
                    .Skip(options.PageIndex.Value * options.PageSize)
                    .Take(options.PageSize);
            }

            var data = await query.ToListAsync();

            return new PagedResult<Leave>
            {
                TotalCount = totalCount,
                Data = data
            };
        }
        public async Task<PagedResult<Leave>> GetByEmployeeIdAsync(int employeeId, SearchOptions options)
        {

            var query = _context.Leaves.Where(x => x.EmployeeId == employeeId);

            var totalCount = await query.CountAsync();

            if (options.PageIndex.HasValue)
            {
                query = query
                    .Skip(options.PageIndex.Value * options.PageSize)
                    .Take(options.PageSize);
            }

            var data = await query.ToListAsync();

            return new PagedResult<Leave>
            {

                TotalCount = totalCount,
                Data = data
            };
        }
    }
}
