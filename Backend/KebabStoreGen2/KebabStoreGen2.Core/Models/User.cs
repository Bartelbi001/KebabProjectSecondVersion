using CSharpFunctionalExtensions;

namespace KebabStoreGen2.Core.Models;

public class User
{
    public const int MAX_USERNAME_LENGTH = 32;

    private User(Guid id, string userName, string passwordHash, Email email)
    {
        Id = id;
        UserName = userName;
        PasswordHash = passwordHash;
        Email = email;
    }

    public Guid Id { get; }
    public string UserName { get; } = string.Empty;
    public string PasswordHash { get; } = string.Empty;
    public Email Email { get; }

    public static Result<User> Create(Guid id, string userName, string passwordHash, Email email)
    {
        if (string.IsNullOrWhiteSpace(userName) || userName.Length > MAX_USERNAME_LENGTH)
        {
            return Result.Failure<User>($"'{nameof(userName)}' can't be null or empty and must be shorter than {MAX_USERNAME_LENGTH}");
        }

        if (string.IsNullOrWhiteSpace(passwordHash))
        {
            return Result.Failure<User>($"'{nameof(passwordHash)}' can't be null or empty");
        }

        return Result.Success(new User(id, userName, passwordHash, email));
    }
}