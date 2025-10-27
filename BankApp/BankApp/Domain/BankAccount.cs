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
        /// 
        /// </summary>
        /// <param name="amount"></param>
        /// <exception cref="ArgumentException"></exception>
        public void Deposit(decimal amount)
        {
            if (amount <= 0) throw new ArgumentException("Amount must be positive");
            Balance += amount;
            LastUpdated = DateTime.Now;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="amount"></param>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public void Withdraw(decimal amount)
        {
            if (amount <= 0) throw new ArgumentException("Amount must be positive");
            if (amount > Balance) throw new InvalidOperationException("Insufficient funds");
            Balance -= amount;
            LastUpdated = DateTime.Now;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="transaction"></param>
        /// <exception cref="ArgumentNullException"></exception>
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
