using static ATM.JsonFile;

namespace ATM;

public class User()
{
    public Guid UserId { get; set; } = Guid.NewGuid();
    public string UserName { get; set; }
    public string Password { get; set; }
    public Card UserCard { get; set; } = new();

    private User(string userName, string password)
        : this()
    {
        UserName = userName;
        Password = password;
    }

    public static User Login()
    {
        var rng = new Random();
        var validCount = 0;
        var isValid = true;
        Console.Write("Enter your name: ");
        var username = Console.ReadLine();

        Console.WriteLine("\nChecking, please wait...\n(Usually takes about 1 to 5 seconds)");
        Thread.Sleep(rng.Next(1000, 5000));
        var users = GetAllUsernames();
        if (users.Exists(name => name == username))
        {
            var user = LoadUser(username);
            do
            {
                Console.Write("User found, please enter your password: ");
                var password = Console.ReadLine();
                if (password != user.Password)
                {
                    if (validCount > 1)
                        isValid = false;

                    validCount++;
                }
                else
                {
                    Console.WriteLine($"{user.UserName} logged in successfully!");
                    return user;
                }
            } while (isValid);
        }

        if (!isValid)
            return null;

        Console.WriteLine("No user found!");
        var result = Register(username);

        SaveUserToFile(result);

        return result;
    }

    private static User Register(string username)
    {
        Console.WriteLine($"Registering a new user with name: {username}\n");

        Console.Write("Please enter your desired password: ");
        var pass = Console.ReadLine();

        return new User(username, pass);
    }

    public Card GetCurrentCard()
    {
        var current = LoadUser(UserName);

        return UserCard.CardId == current.UserCard.CardId ? UserCard : null;
    }
}
