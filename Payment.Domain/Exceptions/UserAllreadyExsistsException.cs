namespace Payment.Domain.Exceptions;

public class UserAllreadyExsistsException : Exception
{
    public UserAllreadyExsistsException(string value)
        : base($"User with this {(value.Contains('@') ? "email" : "phone")} [{value}] allready exsists.")
    {

    }
}
