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

    public static Card GetCurrentCard(User user)
    {
        var current = LoadUser(user.UserName);

        return user.UserCard.CardId == current.UserCard.CardId ? user.UserCard : null;
    }

    public static void Withdraw(User user, int amount)
    {
        if (!user.UserCard.IsLimitReached)
        {
            // Update LastUsed time
            user.UserCard.LastUsed = DateOnly.FromDateTime(DateTime.Now);

            // Remove amount (if available)
            if (user.UserCard.MoneyAmount < amount)
            {
                Console.WriteLine($"Sorry, you don't have enough funds to withdraw {amount}!");
                return;
            }

            user.UserCard.MoneyAmount -= amount;

            // Add transaction to limit
            user.UserCard.UpdateTransactionList(
                $"[{user.UserCard.LastUsed}] Withdraw: {amount} was withdrawn. Remaining amount => ${user.UserCard.MoneyAmount}"
            );

            // Update transaction limits
            user.UserCard.TransactionCount++;
            user.UserCard.UpdateLimit();
        }

        // Save data to file
        SaveUserToFile(user);
    }

    public static void Deposit(User user, int amount)
    {
        if (!user.UserCard.IsLimitReached)
        {
            user.UserCard.LastUsed = DateOnly.FromDateTime(DateTime.Now);

            user.UserCard.MoneyAmount += amount;

            // Add transaction to limit
            user.UserCard.UpdateTransactionList(
                $"[{user.UserCard.LastUsed}] Deposit: {amount} was deposited. Remaining amount => ${user.UserCard.MoneyAmount}."
            );

            // Update transaction limits
            user.UserCard.TransactionCount++;
            user.UserCard.UpdateLimit();
        }

        // Save data to file
        SaveUserToFile(user);
    }
}
