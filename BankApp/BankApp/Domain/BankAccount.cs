
namespace BankApp.Domain;

public class BankAccount : IBankAccount
{
    public Guid id { get;  private set  } = Guid.NewGuid();

    public string Name { get; private set }

    public string Currency { get; private set }

    public decimal Balance { get; private set }

    public DateTime LastUpdated { get; private set }

    public void deposit(decimal amount)
    {
        throw new NotImplementedException();
    }

    public void withdraw(decimal amount)
    {
        throw new NotImplementedException();
    }
}
