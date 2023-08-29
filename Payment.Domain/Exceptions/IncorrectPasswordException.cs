namespace Payment.Domain.Exceptions;


public class IncorrectPasswordException : Exception
{
    public IncorrectPasswordException(string email) : base($"Incorrect password for '{email}'") { }
}
