using DotNetAdvance.Interface;
using DotNetAdvance.Models;

namespace DotNetAdvance.Services
{
    public class ToDoItemUpdatedService : IToDoItemUpdated
    {
        private readonly IDatabaseRepository<ToDoItemUpdated> _context;

        public ToDoItemUpdatedService(IDatabaseRepository<ToDoItemUpdated> context)
        {
            _context = context;
        }
        public async Task<IEnumerable<ToDoItemUpdated>> GetToDoItemsAsync()
        {
            return await _context.GetItemsAsync();
        }
        public async Task<ToDoItemUpdated> GetToDoItemAsync(int id)
        {
            return await _context.GetItemByIdAsync(id);
        }

        public async Task<int> AddToDoItem(ToDoItemUpdated item)
        {
            return await _context.AddItemAsync(item);
        }

        public async Task<int> UpdateToDoItem(int id, ToDoItemUpdated item)
        {
            return await _context.UpdateItemAsync(id, item);
        }

        public async Task<int> DeleteToDoItemAsync(int id)
        {
            return await _context.DeleteItemAsync(id);
        }
    }
}
