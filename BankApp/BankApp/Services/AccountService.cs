using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

            // Hämta konton från storage — använd BankAccount, inte IBankAccount, för JSON-deserialisering
            var fromStorage = await _storageService.GetItemAsync<List<BankAccount>>(StorageKey);

            _accounts.Clear();

            if (fromStorage is { Count: > 0 })
            {
                foreach (var account in fromStorage)
                    _accounts.Add(account);
            }

            isLoaded = true;
        }

        private async Task SaveAsync()
        {
            // JSON kan inte serialisera interfaces → casta till konkreta typer
            var concreteAccounts = _accounts.OfType<BankAccount>().ToList();
            await _storageService.SetItemAsync(StorageKey, concreteAccounts);
        }

        public async Task<IBankAccount> CreateAccount(string name, string currency, decimal balance, AccountType accountType)
        {
            await InitializeAsync();

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
    }
}
