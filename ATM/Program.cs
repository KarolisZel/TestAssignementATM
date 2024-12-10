using static ATM.Card;
using static ATM.JsonFile;
using static ATM.User;

namespace ATM;

class Program
{
    static void Main(string[] args)
    {
        // Login or Create a new user
        var currentUser = Login();
        if (currentUser is null)
            return;

        // Get current users card
        var currentCard = GetCurrentCard(currentUser);
        if (currentCard is null)
            Console.WriteLine($"Problem with your card {currentCard}");

        SaveUserToFile(currentUser);

        Deposit(currentUser, 20);
        Deposit(currentUser, 15);
        Deposit(currentUser, 5);
        Withdraw(currentUser, 10);

        LoadAllUsers();
    }
}
