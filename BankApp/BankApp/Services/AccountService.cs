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
            Console.WriteLine("AccountService created.");
            _storageService = storageService;
        }

        /// <summary>
        /// Initializes the account list by loading data from persistent storage if not already loaded.
        /// </summary>
        /// <returns>A task that represents the asynchronous initialization operation.</returns>
        private async Task InitializeAsync()
        {
            Console.WriteLine("Initializing accounts...");
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
        /// Saves all current accounts to persistent storage.
        /// </summary>
        /// <returns>A task that represents the asynchronous save operation.</returns>
        private async Task SaveAsync()
        {
            Console.WriteLine("Saving accounts...");
            var concreteAccounts = _accounts.OfType<BankAccount>().ToList();
            await _storageService.SetItemAsync(StorageKey, concreteAccounts);
        }

        /// <summary>
        /// Creates a new bank account with the specified details and saves it to storage.
        /// </summary>
        /// <param name="name">The name of the account.</param>
        /// <param name="currency">The currency used for the account.</param>
        /// <param name="balance">The initial balance of the account.</param>
        /// <param name="accountType">The type of account (e.g., savings or deposit).</param>
        /// <returns>The newly created <see cref="IBankAccount"/> instance.</returns>
        /// <exception cref="InvalidOperationException">Thrown when an account with the same name already exists.</exception>
        public async Task<IBankAccount> CreateAccount(string name, string currency, decimal balance, AccountType accountType)
        {
            Console.WriteLine($"Creating account '{name}'...");
            await InitializeAsync();

            if (_accounts.Any(a => a.Name == name))
                throw new InvalidOperationException($"An account with the name '{name}' already exists.");

            var account = new BankAccount(name, currency, balance, accountType);
            _accounts.Add(account);

            await SaveAsync();
            return account;
        }

        /// <summary>
        /// Retrieves an account based on its unique ID.
        /// </summary>
        /// <param name="id">The unique identifier of the account.</param>
        /// <returns>The matching <see cref="IBankAccount"/> if found.</returns>
        /// <exception cref="KeyNotFoundException">Thrown when no account is found with the specified ID.</exception>
        public async Task<IBankAccount?> GetAccount(Guid id)
        {
            Console.WriteLine($"Fetching account {id}...");
            await InitializeAsync();

            var account = _accounts.FirstOrDefault(a => a.Id == id);

            if (account is null)
                throw new KeyNotFoundException($"Account with id {id} not found.");

            return account;
        }

        /// <summary>
        /// Retrieves all existing bank accounts.
        /// </summary>
        /// <returns>A list containing all <see cref="IBankAccount"/> instances.</returns>
        public async Task<List<IBankAccount>> GetAllAccounts()
        {
            Console.WriteLine("Fetching all accounts...");
            await InitializeAsync();

            return _accounts.ToList();
        }

        /// <summary>
        /// Deletes a bank account based on its unique ID.
        /// </summary>
        /// <param name="id">The unique identifier of the account to delete.</param>
        /// <returns>A task that represents the asynchronous delete operation.</returns>
        /// <exception cref="KeyNotFoundException">Thrown when no account is found with the specified ID.</exception>
        public async Task DeleteAccount(Guid id)
        {
            Console.WriteLine($"Deleting account {id}...");
            await InitializeAsync();

            var acc = _accounts.FirstOrDefault(a => a.Id == id)
                      ?? throw new KeyNotFoundException($"Account {id} not found.");

            _accounts.Remove(acc);
            await SaveAsync();
            Console.WriteLine($"Account {id} deleted.");
        }

        /// <summary>
        /// Saves the current list of bank accounts asynchronously to persistent storage.
        /// </summary>
        /// <remarks>
        /// This method filters the accounts to include only those of type <see cref="BankAccount"/> before saving.
        /// The accounts are stored using the specified storage service.
        /// </remarks>
        /// <returns>A task that represents the asynchronous save operation.</returns>
        public async Task SaveAccountsAsync()
        {
            Console.WriteLine("Saving accounts...");
            await SaveAsync();
            var concreteAccounts = _accounts.OfType<BankAccount>().ToList();
            await _storageService.SetItemAsync(StorageKey, concreteAccounts);
            Console.WriteLine("Accounts saved.");
        }
    }
}
