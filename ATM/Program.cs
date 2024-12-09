using static ATM.JsonFile;
using static ATM.User;

namespace ATM;

class Program
{
    static void Main(string[] args)
    {
        var users = new List<User>();

        var alice = CreateUser("Alice", "password123");
        users.Add(alice);

        var bob = CreateUser("Bob", "secure456");
        users.Add(bob);

        var billy = CreateUser("Billy", "test");
        users.Add(billy);

        CheckUser(billy.UserName);

        SaveUserToFile(users);

        LoadAllUsers();
    }

    static User CheckUser(string username)
    {
        // Get all users
        var users = GetAllUsernames();

        // Check if User with this username exists
        if (users.Exists(name => name == username))
            // Check password
            return LoadUser(username);

        // If it does not exist => Create a new user

        return CreateUser(username, "InputPasswordHere");
    }
}
