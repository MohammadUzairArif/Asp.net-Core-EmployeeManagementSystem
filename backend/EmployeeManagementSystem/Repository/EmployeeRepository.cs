using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Helpers;
using EmployeeManagementSystem.Interfaces;
using EmployeeManagementSystem.Model;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementSystem.Repository
{
    public class EmployeeRepository : Repository<Employee>, IEmployeeRepository
    {
        private readonly ApplicationDBContext _context;

        public EmployeeRepository(ApplicationDBContext context) : base(context)
        {
            _context = context;
        }

        public async Task<PagedResult<Employee>> GetAllAsync(SearchOptions options)
        {
            var query = _context.Employees.AsQueryable();

            // Apply search
            if (!string.IsNullOrWhiteSpace(options.Search))
            {
                query = query.Where(x =>
                    x.Name.Contains(options.Search) ||
                    x.Phone.Contains(options.Search) ||
                    x.Email.Contains(options.Search));
            }

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

            return new PagedResult<Employee>
            {
                TotalCount = totalCount,
                Data = data
            };
        }


    }
}
