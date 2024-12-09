namespace ATM;

public class Card
{
    public Card(string cardName)
    {
        CardName = cardName;
    }

    public Guid CardId { get; set; } = Guid.NewGuid();
    public string CardName { get; set; }
    public string Pin { get; set; } = "1111";
    public decimal MoneyAmount { get; set; }
    public List<string> TransactionList { get; set; } = [];
    public bool IsLimitReached { get; set; }
    public DateOnly LastUsed { get; set; }

    private void UpdateLastUsed()
    {
        LastUsed = DateOnly.FromDateTime(DateTime.Now);
    }

    private void UpdateTransactionList(string transaction)
    {
        if (TransactionList.Count > 5)
        {
            TransactionList.RemoveAt(0);
            TransactionList.Add(transaction);
        }
        else
        {
            TransactionList.Add(transaction);
        }
    }

    private void UpdateLimit(int transactionCount)
    {
        IsLimitReached = transactionCount >= 10;
    }
}
