namespace BankApp.Interfaces
{
    /// <summary>
    /// Interface for account-related operations.
    /// </summary>
    public interface IAccountService
    {
        Task<IBankAccount> CreateAccount(string name, string currency, decimal initialBalance, AccountType accountType);
        Task<List<IBankAccount>> GetAllAccounts();
        Task<IBankAccount?> GetAccount(Guid id);
        Task DeleteAccount(Guid id);
        Task SaveAccountsAsync();
    }
}
