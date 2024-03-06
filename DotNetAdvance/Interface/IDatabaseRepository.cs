namespace DotNetAdvance.Interface
{
    public interface IDatabaseRepository<T>
    {
        Task<IEnumerable<T>> GetItemsAsync();
        Task<T> GetItemByIdAsync(int id);
        Task<int> AddItemAsync(T item);
        Task<int> UpdateItemAsync(int id, T item);
        Task<int> DeleteItemAsync(int id);
    }
}
