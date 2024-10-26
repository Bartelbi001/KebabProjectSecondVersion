using CSharpFunctionalExtensions;
using System.Net.Mail;


namespace KebabStoreGen2.Core.Models;

public class Email
{
    public const int MAX_EMAIL_LENGTH = 254;

    private Email(string value)
    {
        Value = value;
    }

    public string Value { get; } = string.Empty;

    public static Result<Email> Create(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return Result.Failure<Email>("Email is required");
        }

        try
        {
            var mailAdress = new MailAddress(email);
            if (mailAdress.Address != email)
            {
                return Result.Failure<Email>("Invalid email format");
            }
        }
        catch (Exception)
        {

            return Result.Failure<Email>("Invalid email format");
        }

        if (email.Length > MAX_EMAIL_LENGTH)
        {
            return Result.Failure<Email>("Email must be shorter than 254 characters");
        }

        return Result.Success(new Email(email));
    }
}
