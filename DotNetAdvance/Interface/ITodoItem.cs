using DotNetAdvance.Models;

namespace DotNetAdvance.Interface
{
    public interface ITodoItem
    {
        Task<IEnumerable<ToDoItem>> GetToDoItemsAsync();
        Task<ToDoItem> GetToDoItemAsync(int id);
        Task<int> AddToDoItem(ToDoItem item);
        Task<int> UpdateToDoItem(int id, ToDoItem item);
        Task<int> DeleteToDoItemAsync(int id);
    }
}
