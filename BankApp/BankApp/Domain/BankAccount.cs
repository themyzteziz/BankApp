
using System.Transactions;

namespace BankApp.Domain;

public class BankAccount : IBankAccount
{
    public Guid id { get; private set; } = Guid.NewGuid();

    public string Name { get; private set; }

    public string Currency { get; private set; }

    public decimal Balance { get; private set; }

    public DateTime LastUpdated { get; private set; }
    public AccountType AccountType { get; private set; }

    private readonly List<Transaction> _transactions = new();
    public IReadOnlyList<Transaction> Transactions => _transactions;

    public BankAccount(string name, string currency, AccountType accountType, decimal initialBalance)
    {
        Name = name;
        Currency = currency;
        Balance = initialBalance;
        LastUpdated = DateTime.UtcNow;
        AccountType = accountType;

    }

    public void deposit(decimal amount)
    {
        throw new NotImplementedException();
    }

    public void withdraw(decimal amount)
    {
        throw new NotImplementedException();
    }
    public void AddTransaction(Transaction transaction)
    {
        if (transaction is null) throw new ArgumentNullException(nameof(transaction));
        _transactions.Add(transaction);

        // uppdatera saldo
        Balance += transaction.Type switch
        {
            TransactionType.Deposit => transaction.Amount,
            TransactionType.Withdrawal => -transaction.Amount,
            _ => 0m
        };
    }
}
