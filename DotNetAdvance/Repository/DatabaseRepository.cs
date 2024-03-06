using DotNetAdvance.DatabaseContext;
using DotNetAdvance.Interface;
using Microsoft.EntityFrameworkCore;

namespace DotNetAdvance.Repository
{
    public class DatabaseRepository<T> : IDatabaseRepository<T> where T : class
    {
        private readonly DefaultContext _context ;
        private readonly DbSet<T> _dbSet ;

        public DatabaseRepository(DefaultContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }
        public async Task<IEnumerable<T>> GetItemsAsync()
        {
            return await _dbSet.ToListAsync();
        }
        public async Task<T> GetItemByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<int> AddItemAsync(T item)
        {
            await _dbSet.AddAsync(item);
            return await _context.SaveChangesAsync();
           
        }
        public async Task<int> UpdateItemAsync(int id, T item)
        {
            _context.Entry(item).State = EntityState.Modified;
            return await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteItemAsync(int id)
        {
            var toDoItem = await _dbSet.FindAsync(id);
            _dbSet.Remove(toDoItem);
            return await _context.SaveChangesAsync();
        }



    }
}
