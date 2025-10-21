using System.Linq.Expressions;

namespace EmployeeManagementSystem.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<List<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);
        Task AddAsync(T entity);
        void Update(T entity);
        Task DeleteAsync(int id);
        Task<int> SaveChangesAsync();
        Task<T?> FindAsync(Expression<Func<T, bool>> predicate);
        Task<IEnumerable<T>> FindAsyncList(Expression<Func<T, bool>> predicate);
    }
}
