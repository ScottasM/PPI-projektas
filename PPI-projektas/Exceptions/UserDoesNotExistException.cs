using System;
namespace PPI_projektas.Exceptions;

public class UserDoesNotExistException : Exception
{
    public UserDoesNotExistException()
    {
    }

    public UserDoesNotExistException(string message)
        : base(message)
    {
    }

    public UserDoesNotExistException(string message, Exception inner)
        : base(message, inner)
    {
    }

    public UserDoesNotExistException(Guid ownerId)
        : base($"User with ID {ownerId} does not exist!")
    {
    }
}