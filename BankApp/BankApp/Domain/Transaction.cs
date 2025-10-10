namespace BankApp.Domain
{
    public class Transaction
    {
        public Guid Id { get; private set; } = Guid.NewGuid();

        public DateTime Date { get; private set; } = DateTime.Now;
        public decimal Amount { get; private set; }
        public string Description { get; private set; } = string.Empty;
        public string Currency { get; private set; } = "SEK";
        public TransactionType Type { get; private set; }

        public Transaction(decimal amount, string description, TransactionType type, string currency = "SEK")
        {
            Amount = amount;
            Description = description;
            Type = type;
            Currency = currency;
        }
    }

    public enum TransactionType
    {
        Deposit,
        Withdrawal,
        Transfer
    }
}
