using static ATM.JsonFile;

namespace ATM;

public class Card()
{
    public Guid CardId { get; set; } = Guid.NewGuid();
    public string Pin { get; set; }
    public decimal MoneyAmount { get; set; }
    public int TransactionCount { get; set; }
    public List<string> TransactionList { get; set; } = [];
    public bool IsLimitReached { get; set; }
    public DateOnly LastUsed { get; set; }

    private void UpdateTransactionList(string transaction)
    {
        if (TransactionList.Count == 5)
        {
            TransactionList.RemoveAt(0);
            TransactionList.Add(transaction);
        }
        else
        {
            TransactionList.Add(transaction);
        }
    }

    private void UpdateLimit()
    {
        IsLimitReached = TransactionCount >= 10;
    }

    private void CheckLimitReset()
    {
        if (LastUsed >= DateOnly.FromDateTime(DateTime.Now) && TransactionCount != 0)
            return;

        TransactionCount = 0;
        IsLimitReached = false;
    }

    public void Withdraw(int amount)
    {
        CheckLimitReset();

        if (!IsLimitReached)
        {
            // Update LastUsed time
            LastUsed = DateOnly.FromDateTime(DateTime.Now);

            // Remove amount (if available)
            if (MoneyAmount < amount)
            {
                Console.WriteLine($"Sorry, you don't have enough funds to withdraw {amount} Moni!");
                Thread.Sleep(3500);
                return;
            }

            if (amount > 1000)
            {
                Console.WriteLine($"Sorry, withdraw limit is 1000 Moni!");
                Thread.Sleep(3500);
                return;
            }

            MoneyAmount -= amount;

            // Add transaction to limit
            UpdateTransactionList(
                $"[{LastUsed}] Withdraw: {amount} Moni was withdrawn. Remaining amount => ${MoneyAmount} Moni"
            );

            // Update transaction limits
            TransactionCount++;
            UpdateLimit();
            Console.WriteLine($"{amount} Moni was successfully withdrawn.");

            Thread.Sleep(3500);
            return;
        }

        Console.WriteLine("Transaction limit reached!");
        Thread.Sleep(3500);
    }

    public void Deposit(int amount)
    {
        CheckLimitReset();

        if (!IsLimitReached)
        {
            LastUsed = DateOnly.FromDateTime(DateTime.Now);

            MoneyAmount += amount;

            // Add transaction to limit
            UpdateTransactionList(
                $"[{LastUsed}] Deposit: {amount} Moni was deposited. Remaining amount => ${MoneyAmount} Moni"
            );

            // Update transaction limits
            TransactionCount++;
            UpdateLimit();
            Console.WriteLine($"{amount} Moni was successfully deposited.");

            Thread.Sleep(3500);
            return;
        }

        Console.WriteLine("Transaction limit reached!");
        Thread.Sleep(3500);
    }

    public void PrintData()
    {
        Console.WriteLine($"\tBalance => {MoneyAmount} Moni");
        Console.WriteLine($"\tTransactions today => {TransactionCount}");
        Console.WriteLine("\tTransaction list => ");
        Console.WriteLine("\t" + string.Join(",\n\t", TransactionList));
        Console.WriteLine($"\tLast used => {LastUsed}");
    }
}
