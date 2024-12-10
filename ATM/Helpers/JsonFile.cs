using System.Text.Json;

namespace ATM;

static class JsonFile
{
    private const string FileFolderPath = @"C:\Users\koral\RiderProjects\ATM\ATM\UserData";

    private static readonly JsonSerializerOptions JsonOptions = new() { WriteIndented = true };

    public static void LoadAllUsers()
    {
        var users = new List<User>();

        if (!Directory.Exists(FileFolderPath))
        {
            Console.WriteLine($"Warning: Directory {FileFolderPath} does not exist!");
            return;
        }

        var files = Directory.GetFiles(FileFolderPath, "*.json");
        foreach (var file in files)
        {
            var user = LoadUserFromFile(file);
            users.Add(user);
        }

        foreach (var user in users)
        {
            Console.WriteLine($"UserName: {user.UserName}");

            Console.WriteLine($"\tMoney: {user.UserCard.MoneyAmount}");
            Console.WriteLine($"\tTransactions: ");
            Console.WriteLine("" + string.Join(",\n", user.UserCard.TransactionList));
        }
    }

    public static void SaveUserToFile(User user)
    {
        Directory.CreateDirectory(FileFolderPath);

        var filePath = Path.Combine(FileFolderPath, $"{user.UserName}.json");
        File.Create(filePath).Close();

        var json = JsonSerializer.Serialize(user, JsonOptions);

        File.WriteAllText(filePath, json);
    }

    private static User LoadUserFromFile(string filePath)
    {
        try
        {
            var json = File.ReadAllText(filePath);

            var user = JsonSerializer.Deserialize<User>(json, JsonOptions);

            if (user == null)
            {
                Console.WriteLine(
                    $"Warning: The file '{filePath}' could not be deserialized into a User object."
                );
            }

            return user;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading user from file '{filePath}': {ex.Message}");
            return null;
        }
    }

    public static User LoadUser(string username)
    {
        var filePath = $@"{FileFolderPath}\{username}.json";

        try
        {
            var json = File.ReadAllText(filePath);

            var user = JsonSerializer.Deserialize<User>(json, JsonOptions);

            if (user == null)
            {
                Console.WriteLine(
                    $"Warning: The file '{filePath}' could not be deserialized into a User object."
                );
            }

            return user;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading user from file '{filePath}': {ex.Message}");
            return null;
        }
    }

    public static List<string> GetAllUsernames()
    {
        var usernames = new List<string>();
        if (!Directory.Exists(FileFolderPath))
            return usernames;

        var files = Directory.GetFiles(FileFolderPath, "*.json");

        foreach (var file in files)
        {
            var fileSplit = file.Split(@"\");
            var usernameSplit = fileSplit[^1].Split('.');
            usernames.Add(usernameSplit[0]);
        }

        return usernames;
    }
}
