namespace BankApp.Interfaces
{
    /// <summary>
    /// Interface for storage-related operations.
    /// </summary>
    public interface IStorageService
    {
        Task SetItemAsync<T>(string key, T value);
        Task<T?> GetItemAsync<T>(string key);
    }
}