namespace BankApp.Interfaces
{
    public interface IAccountService
    {
        IBankAccount CreateAccount(string name, string currency, decimal initialBalance, AccountType accountType);
        List<IBankAccount> GetAllAccounts();
    }
}
