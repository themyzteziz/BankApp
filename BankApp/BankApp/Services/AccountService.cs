using BankApp.Domain;

namespace BankApp.Services
{
    public class AccountService : IAccountService
    {
        private readonly List<IBankAccount> _accounts;

        public AccountService()
        {
            _accounts = new List<IBankAccount>();
        }

        public IBankAccount CreateAccount(string name, string currency, decimal initialBalance, AccountType accountType)
        {
            var account = new BankAccount(name, currency, accountType, initialBalance);
            _accounts.Add(account);
            return account;
        }

        public List<IBankAccount> GetAllAccounts() => _accounts;

        

    }
}
