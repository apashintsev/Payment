namespace Payment.Domain.Exceptions;

public class UserNotFoundException : Exception
{
    public UserNotFoundException(string email) : base($"User '{email}' not found.") { }
}
