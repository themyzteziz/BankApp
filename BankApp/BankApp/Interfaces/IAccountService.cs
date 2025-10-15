namespace BankApp.Interfaces
{
    public interface IAccountService
    {
        Task<IBankAccount> CreateAccount(string name, string currency, decimal initialBalance, AccountType accountType);
        Task<List<IBankAccount>> GetAllAccounts();
        Task<IBankAccount?> GetAccount(Guid id);
        Task DeleteAccount(Guid id);
    }
}
