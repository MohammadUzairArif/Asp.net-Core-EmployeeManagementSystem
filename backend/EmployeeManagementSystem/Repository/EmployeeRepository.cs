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

        public async Task<List<Employee>> GetAllAsync(SearchOptions options)
        {
            var query = _context.Employees.AsQueryable();

            // Apply search if given
            if (!string.IsNullOrWhiteSpace(options.Search))
            {
                query = query.Where(x =>
                    x.Name.Contains(options.Search) ||
                    x.Phone.Contains(options.Search) ||
                    x.Email.Contains(options.Search));
            }

            // Apply pagination only if both PageIndex  are given
            if (options.PageIndex.HasValue)
            {
                query = query
                    .Skip(options.PageIndex.Value * options.PageSize)
                    .Take(options.PageSize);
            }

            return await query.ToListAsync();
        }


    }
}
