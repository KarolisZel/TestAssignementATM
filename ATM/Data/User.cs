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

    // public static User CreateUser(string userName, string password)
    // {
    //     var user = new User(userName, password);
    //     return user;
    // }

    public static User Login()
    {
        var validCount = 0;
        var isValid = true;
        Console.Write("Enter your username: ");
        var username = Console.ReadLine();

        var users = GetAllUsernames();
        if (users.Exists(name => name == username))
        {
            var user = LoadUser(username);
            do
            {
                Console.Write("Enter your password: ");
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
        Console.WriteLine($"Creating a new user with username: {username}\n");

        Console.Write("Please enter your desired password: ");
        var pass = Console.ReadLine();

        return new User(username, pass);
    }
}
