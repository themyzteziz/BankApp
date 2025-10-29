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
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private async Task SaveAsync()
        {

            var concreteAccounts = _accounts.OfType<BankAccount>().ToList();
            await _storageService.SetItemAsync(StorageKey, concreteAccounts);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="currency"></param>
        /// <param name="balance"></param>
        /// <param name="accountType"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        public async Task<IBankAccount?> GetAccount(Guid id)
        {
            await InitializeAsync();

            var account = _accounts.FirstOrDefault(a => a.Id == id);

            if (account is null)
                throw new KeyNotFoundException($"Account with id {id} not found.");

            return account;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<List<IBankAccount>> GetAllAccounts()
        {
            await InitializeAsync();

            return _accounts.ToList();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        public async Task DeleteAccount(Guid id)
        {
            await InitializeAsync();

            var acc = _accounts.FirstOrDefault(a => a.Id == id)
                      ?? throw new KeyNotFoundException($"Account {id} not found.");

            _accounts.Remove(acc);
            await SaveAsync();
        }

        /// <summary>
        /// Asynchronously saves the current state of accounts to persistent storage.
        /// </summary>
        /// <remarks>This method performs an asynchronous operation to persist account data.  Ensure that
        /// any changes to account data are complete before calling this method.</remarks>
        /// <returns>A task that represents the asynchronous save operation.</returns>
        public async Task SaveAccountsAsync()
        {
            await SaveAsync();
            var concreteAccounts = _accounts.OfType<BankAccount>().ToList();
            await _storageService.SetItemAsync(StorageKey, concreteAccounts);
        }
    }
}