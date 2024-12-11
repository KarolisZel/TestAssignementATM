using static ATM.JsonFile;

namespace ATM;

class AtmMachine
{
    private static Random random = new();
    private static User? CurrentUser { get; set; }

    public static void ATM()
    {
        Console.Clear();
        Console.WriteLine("Welcome to the ATM!");

        CurrentUser = User.Login();

        while (CurrentUser is not null)
        {
            PrintMenu();

            var selection = Console.ReadKey(true);

            switch (selection.Key)
            {
                case ConsoleKey.D1:
                    // Show balance
                    Console.Clear();
                    ShowBalance();
                    break;

                case ConsoleKey.D2:
                    // Deposit
                    Console.Clear();
                    ExecuteDeposit();
                    break;

                case ConsoleKey.D3:
                    // Withdraw
                    Console.Clear();
                    ExecuteWithdraw();
                    break;

                case ConsoleKey.D4:
                    // Show all data
                    Console.Clear();
                    PrintCardData();
                    break;

                case ConsoleKey.C:
                    // Quit
                    CurrentUser = null;

                    Console.Clear();
                    Console.WriteLine("Thank you for using our services!");
                    break;

                default:
                    // Default should never happen
                    Console.WriteLine("This MUST never happen!");
                    break;
            }
        }

        if (CurrentUser is null)
        {
            Console.WriteLine("The machine will be accessible in a few moments, please wait...");
            Thread.Sleep(random.Next(1000, 5000));
            ATM();
        }
    }

    private static void PrintHeader()
    {
        Console.Clear();
        Console.WriteLine($"Welcome {CurrentUser.UserName}!\n\n");
    }

    private static void PrintMenu()
    {
        PrintHeader();

        Console.WriteLine("Please select your action!");
        Console.WriteLine("1. Show balance");
        Console.WriteLine("2. Deposit");
        Console.WriteLine("3. Withdraw");
        Console.WriteLine("4. Show all data");
        Console.WriteLine("\n\nC. Cancel and return card");
    }

    private static void ShowBalance()
    {
        WaitFive();
        PrintHeader();

        Console.WriteLine("Your current balance:");
        Console.WriteLine($"\t{CurrentUser.UserCard.MoneyAmount} Moni");

        Console.WriteLine("\n\nPress Q to return.");
        GoBack();
    }

    private static void GoBack()
    {
        // Wait for correct key press
        var k = Console.ReadKey(true);
        while (k.Key != ConsoleKey.Q)
        {
            k = Console.ReadKey(true);
        }
    }

    private static void ExecuteDeposit()
    {
        WaitFive();
        PrintHeader();

        Console.Write("Enter desired amount: ");
        var success = int.TryParse(Console.ReadLine(), out var amount);

        while (!success)
        {
            Console.WriteLine(
                "Incorrect format is used to specify the amount!\n\t(number expected)"
            );
            Console.Write("Please try again: ");
            success = int.TryParse(Console.ReadLine(), out amount);
        }

        CurrentUser.UserCard.Deposit(amount);

        // Save data to file
        SaveUserToFile(CurrentUser);
    }

    private static void ExecuteWithdraw()
    {
        WaitFive();
        PrintHeader();

        Console.Write("Enter desired amount: ");
        var success = int.TryParse(Console.ReadLine(), out var amount);

        while (!success)
        {
            Console.WriteLine(
                "Incorrect format is used to specify the amount!\n\t(number expected)"
            );
            Console.Write("Please try again: ");
            success = int.TryParse(Console.ReadLine(), out amount);
        }

        CurrentUser.UserCard.Withdraw(amount);

        // Save data to file
        SaveUserToFile(CurrentUser);
    }

    private static void PrintCardData()
    {
        WaitFive();
        PrintHeader();

        CurrentUser.UserCard.PrintData();

        Console.WriteLine("\n\nPress Q to return.");
        GoBack();
    }

    private static void WaitFive()
    {
        PrintHeader();

        Console.WriteLine("Loading, please wait...");
        Thread.Sleep(5000);
    }
}
