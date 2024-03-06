using DotNetAdvance.Interface;
using DotNetAdvance.Models;

namespace DotNetAdvance.Services
{
    public class TodoItemService : ITodoItem
    {
        private readonly IDatabaseRepository<ToDoItem> _context;

        public TodoItemService(IDatabaseRepository<ToDoItem> context)
        {
            _context = context;
        }
        public async Task<IEnumerable<ToDoItem>> GetToDoItemsAsync()
        {
            return await _context.GetItemsAsync();
        }
        public async Task<ToDoItem> GetToDoItemAsync(int id)
        {
            return await _context.GetItemByIdAsync(id);
        }

        public async Task<int> AddToDoItem(ToDoItem item)
        {
            return await _context.AddItemAsync(item);
        }

        public async Task<int> UpdateToDoItem(int id, ToDoItem item)
        { 
            return await _context.UpdateItemAsync(id, item);
        }

        public async Task<int> DeleteToDoItemAsync(int id)
        {
            return await _context.DeleteItemAsync(id);
        }
    }
}
