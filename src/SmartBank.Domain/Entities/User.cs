namespace SmartBank.Domain.Entities;

public class User
{
    public Guid Id { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public ICollection<Account> Accounts { get; set; }
        = new List<Account>();

    public ICollection<Notification> Notifications { get; set; }
        = new List<Notification>();

        public ICollection<RefreshToken> RefreshTokens { get; set; }
    = new List<RefreshToken>();
}