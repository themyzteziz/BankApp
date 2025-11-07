using System;
using System.ComponentModel.Design;
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

        /// <summary>
        /// Initializes the account list by loading data from persistent storage if not already loaded.
        /// </summary>
        /// <returns>A task that represents the asynchronous initialization operation.</returns>
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
            Console.WriteLine($"[Bank] Loaded {_accounts.Count} account(s) from local storage.");
        }

        /// <summary>
        /// Saves all current accounts to persistent storage.
        /// </summary>
        /// <returns>A task that represents the asynchronous save operation.</returns>
        private async Task SaveAsync()
        {

            var concreteAccounts = _accounts.OfType<BankAccount>().ToList();
            await _storageService.SetItemAsync(StorageKey, concreteAccounts);

        }

        /// <summary>
        /// Creates a new bank account with the specified details and saves it to storage.
        /// </summary>
        /// <param name="name">The name of the account.</param>
        /// <param name="currency">The currency used for the account.</param>
        /// <param name="balance">The initial balance of the account.</param>
        /// <param name="accountType">The type of account (savings or deposit).</param>
        /// <returns>The newly created <see cref="IBankAccount" instance.</returns>
        /// <exception cref="InvalidOperationException">Thrown when an account with the same name already exists.</exception>
        public async Task<IBankAccount> CreateAccount(string name, string currency, decimal balance, AccountType accountType)
        {

            await InitializeAsync();

            if (_accounts.Any(a => a.Name == name))
                throw new InvalidOperationException($"An account with the name '{name}' already exists.");

            var account = new BankAccount(name, currency, balance, accountType);
            _accounts.Add(account);
            await SaveAsync();

            Console.WriteLine($"Created account: {name} ({accountType}) | ID={account.Id} | Start balance: {balance} {currency}");
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
            await InitializeAsync();
            var acc = _accounts.FirstOrDefault(a => a.Id == id)
                      ?? throw new KeyNotFoundException($"Account {id} not found.");

            _accounts.Remove(acc);
            await SaveAsync();
            Console.WriteLine($"[Bank] Deleted account: {acc.Name} | ID={acc.Id}");
        }

        /// <summary>
        /// Saves the current list of bank accounts to persistent storage.
        /// </summary>
        /// <remarks>
        /// This method filters the accounts to include only those of type <see cref="BankAccount"/> before saving.
        /// The accounts are stored using the specified storage service.
        /// </remarks>
        /// <returns>A task that represents the save operation.</returns>
        public async Task SaveAccountsAsync()
        {
            await SaveAsync();
            var concreteAccounts = _accounts.OfType<BankAccount>().ToList();
            await _storageService.SetItemAsync(StorageKey, concreteAccounts);
            Console.WriteLine("Accounts saved.");
        }

        /// <summary>
        /// Applies monthly interest to all savings accounts based on the specified annual interest rate.
        /// </summary>
        /// <param name="annualRatePercent">The annual interest rate, expressed as a percentage. Must be greater than zero.</param>
        /// <returns>A task that represents the asynchronous operation of applying interest to savings accounts.</returns>
        public async Task ApplyMonthlyInterestAsync(decimal annualRatePercent)
        {
            await InitializeAsync();

            var savingsAccounts = _accounts
                .OfType<BankAccount>()
                .Where(a => a.AccountType == AccountType.Savings)
                .ToList();

            decimal annualRate = 2m;

            foreach (var account in savingsAccounts)
            {
                var monthlyRate = annualRatePercent / 12 / 100;
                var interest = account.Balance * monthlyRate;

                account.Deposit(interest);
                Console.WriteLine($"[Bank] Applied {interest} {account.Currency} interest to account '{account.Name}' (New balance: {account.Balance:F2} {account.Currency})");
            }
            await SaveAsync();
        }
    }
}
