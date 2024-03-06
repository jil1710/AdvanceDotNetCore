using DotNetAdvance.Models;

namespace DotNetAdvance.Interface
{
    public interface IToDoItemUpdated
    {
        Task<IEnumerable<ToDoItemUpdated>> GetToDoItemsAsync();
        Task<ToDoItemUpdated> GetToDoItemAsync(int id);
        Task<int> AddToDoItem(ToDoItemUpdated item);
        Task<int> UpdateToDoItem(int id, ToDoItemUpdated item);
        Task<int> DeleteToDoItemAsync(int id);
    }
}
