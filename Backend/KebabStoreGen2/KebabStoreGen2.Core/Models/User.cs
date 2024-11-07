using CSharpFunctionalExtensions;
using KebabStoreGen2.Core.Constants;

namespace KebabStoreGen2.Core.Models;

public class User
{
    private User(Guid id, string userFirstName, string userSurName, string passwordHash, Email email)
    {
        Id = id;
        UserFirstName = userFirstName;
        UserSurName = userSurName;
        PasswordHash = passwordHash;
        Email = email;
    }

    public Guid Id { get; }
    public string UserFirstName { get; private set; } = string.Empty;
    public string UserSurName { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public Email Email { get; private set; }

    public static Result<User> Create(Guid id, string userFirstName, string userSurName, string passwordHash, Email email)
    {
        if (string.IsNullOrWhiteSpace(userFirstName) || userFirstName.Length > UserConstants.MAX_USERFIRSTNAME_LENGTH)
        {
            return Result.Failure<User>($"'{nameof(userFirstName)}' can't be null or empty and must be shorter than {UserConstants.MAX_USERFIRSTNAME_LENGTH}");
        }

        if (string.IsNullOrWhiteSpace(userSurName) || userSurName.Length > UserConstants.MAX_USERSURNAME_LENGTH)
        {
            return Result.Failure<User>($"'{nameof(userSurName)}' can't be null or empty and must be shorter than {UserConstants.MAX_USERSURNAME_LENGTH}");
        }

        if (string.IsNullOrWhiteSpace(passwordHash))
        {
            return Result.Failure<User>($"'{nameof(passwordHash)}' can't be null or empty");
        }

        return Result.Success(new User(id, userFirstName, userSurName, passwordHash, email));
    }
}