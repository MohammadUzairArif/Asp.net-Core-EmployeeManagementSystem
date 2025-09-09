using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementSystem.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected DbSet<T> dbSet;
        private readonly ApplicationDBContext dBContext;

        public Repository(ApplicationDBContext dBContext)
        {
            dbSet = dBContext.Set<T>();
            this.dBContext = dBContext;
        }

        public async Task<List<T>> GetAllAsync()
        {
          var list =  await dbSet.ToListAsync();
            return list;
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            var entity = await dbSet.FindAsync(id);
            return entity;
        }

       
        public async Task AddAsync(T entity)
        {
           await dbSet.AddAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await dbSet.FindAsync(id);
            dbSet.Remove(entity!);
        }

        public void Update(T entity)
        {
            dbSet.Update(entity);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await dBContext.SaveChangesAsync();
        }
    }
}
