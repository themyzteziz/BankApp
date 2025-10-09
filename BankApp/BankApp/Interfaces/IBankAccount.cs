namespace BankApp.Interfaces
{
    /// <summary>
    /// Interface containing the BankAccount methods
    /// </summary>
    public interface IBankAccount
    {
        Guid id { get; }
        string Name { get; }
        string Currency { get; }
        decimal Balance { get; }
        DateTime LastUpdated { get; }

        void withdraw(decimal amount);
        void deposit(decimal amount);

    }
}
