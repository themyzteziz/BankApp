using System;
using BankApp.Domain;
using BankApp.Interfaces;

namespace BankApp.Services
{
    public class AccountService : IAccountService
    {
        private const string StorageKey = "bankapp.accounts";

        private readonly List<IBankAccount> _accounts = new();
        private readonly IStorageService _storageService;
        private bool isLoaded;

        public AccountService(IStorageService storageService)
        {
            _storageService = storageService;
        }

        private async Task InitializeAsync()
        {
            if (isLoaded)
                return;

            var fromStorage = await _storageService.GetItemAsync<List<BankAccount>>(StorageKey);

            _accounts.Clear();

            if (fromStorage != null && fromStorage.Count > 0)
            {
                
                foreach (var account in fromStorage)
                {
                    if (!_accounts.Any(a => a.Id == account.Id))
                        _accounts.Add(account);
                }
            }

            isLoaded = true;
        }

        private async Task SaveAsync()
        {
            
            var concreteAccounts = _accounts.OfType<BankAccount>().ToList();
            await _storageService.SetItemAsync(StorageKey, concreteAccounts);
        }

        public async Task<IBankAccount> CreateAccount(string name, string currency, decimal balance, AccountType accountType)
        {
            await InitializeAsync();

            
            if (_accounts.Any(a => a.Name == name))
                throw new InvalidOperationException($"Konto med namnet '{name}' finns redan.");

            var account = new BankAccount(name, currency, balance, accountType);
            _accounts.Add(account);

            await SaveAsync();
            return account;
        }

        public async Task<IBankAccount?> GetAccount(Guid id)
        {
            await InitializeAsync();

            var account = _accounts.FirstOrDefault(a => a.Id == id);

            if (account is null)
                throw new KeyNotFoundException($"Account with id {id} not found.");

            return account;
        }

        public async Task<List<IBankAccount>> GetAllAccounts()
        {
            await InitializeAsync();
            
            return _accounts.ToList();
        }

        public async Task DeleteAccount(Guid id)
        {
            await InitializeAsync();

            var acc = _accounts.FirstOrDefault(a => a.Id == id)
                      ?? throw new KeyNotFoundException($"Account {id} not found.");

            _accounts.Remove(acc);
            await SaveAsync();
        }

        
        public async Task SaveAccountsAsync()
        {
            await SaveAsync();
        }
    }
}
