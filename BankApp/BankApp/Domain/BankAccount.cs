using System.Text.Json.Serialization;

namespace BankApp.Domain
{
    public class BankAccount : IBankAccount
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public string Name { get; private set; } = string.Empty;
        public string Currency { get; private set; } = "SEK";
        public decimal Balance { get; private set; }
        public DateTime LastUpdated { get; private set; } = DateTime.Now;
        public AccountType AccountType { get; private set; }

        private readonly List<Transaction> _transactions = new();
        public IReadOnlyList<Transaction> Transactions => _transactions;

        public BankAccount() { }

        public BankAccount(string name, string currency, decimal balance, AccountType accountType)
        {
            Name = name;
            Currency = currency;
            Balance = balance;
            LastUpdated = DateTime.Now;
            AccountType = accountType;
        }

        [JsonConstructor]
        public BankAccount(Guid id, string name, string currency, decimal balance, DateTime lastUpdated, AccountType accountType, IReadOnlyList<Transaction>? transactions = null)
        {
            Id = id;
            Name = name;
            Currency = currency;
            Balance = balance;
            LastUpdated = lastUpdated;
            AccountType = accountType;

            if (transactions is not null)
                _transactions = transactions.ToList();
        }

        /// <summary>
        /// Deposits the specified amount into the account.
        /// </summary>
        /// <param name="amount">The amount to deposit. Must be greater than zero.</param>
        /// <exception cref="ArgumentException">Thrown if the amount is less than or equal to zero.</exception>
        public void Deposit(decimal amount)
        {
            if (amount <= 0) throw new ArgumentException("Amount must be positive");
            Balance += amount;
            LastUpdated = DateTime.Now;
        }

        /// <summary>
        /// Withdraws the specified amount from the account.
        /// </summary>
        /// <param name="amount">The amount to withdraw. Must be greater than zero and less than or equal to the current balance.</param>
        /// <exception cref="ArgumentException">Thrown if the amount is less than or equal to zero.</exception>
        /// <exception cref="InvalidOperationException">Thrown if the balance is insufficient.</exception>
        public void Withdraw(decimal amount)
        {
            if (amount <= 0) throw new ArgumentException("Amount must be positive");
            if (amount > Balance) throw new InvalidOperationException("Insufficient funds");
            Balance -= amount;
            LastUpdated = DateTime.Now;
        }

        /// <summary>
        /// Adds a transaction to the account and updates the balance accordingly.
        /// </summary>
        /// <param name="transaction">The transaction to add.</param>
        /// <exception cref="ArgumentNullException">Thrown if the transaction is null.</exception>
        public void AddTransaction(Transaction transaction)
        {
            if (transaction is null) throw new ArgumentNullException(nameof(transaction));
            if (_transactions.Any(t => t.Id == transaction.Id))
                return;

            _transactions.Add(transaction);

            Balance += transaction.Type switch
            {
                TransactionType.Deposit => transaction.Amount,
                TransactionType.Withdrawal => -transaction.Amount,
                TransactionType.Transfer => 0,
                _ => 0m
            };
            transaction.BalanceAfterTransaction = Balance;

            LastUpdated = DateTime.Now;
        }
    }
}
