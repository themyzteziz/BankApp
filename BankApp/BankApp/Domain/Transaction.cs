namespace BankApp.Domain
{
    // Represents a single financial transaction made on a bank account.
    public class Transaction
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public DateTime Date { get; private set; } = DateTime.Now;
        public decimal Amount { get; private set; }
        public string Description { get; private set; } = string.Empty;
        public string Currency { get; private set; } = "SEK";
        public TransactionType Type { get; private set; }
        public Guid TransferReciever { get; set; } = Guid.Empty;
        public decimal BalanceAfterTransaction { get; set; }    

        public Transaction(decimal amount, string description, TransactionType type, string currency)
        {
            Amount = amount;
            Description = description;
            Type = type;
            Currency = currency;
            Date = DateTime.Now;
        }
    }

    // Defines the possible types of financial transactions.
    public enum TransactionType
    {
        Deposit,
        Withdrawal,
        Transfer
    }
}
