namespace ATM;

public class User(string userName, string password)
{
    public Guid UserId { get; set; } = Guid.NewGuid();
    public string UserName { get; set; } = userName;
    public string Password { get; set; } = password;
    public List<Card> UserCards { get; set; } = new();

    public static User CreateUser(string userName, string password)
    {
        var user = new User(userName, password);
        return user;
    }
}
