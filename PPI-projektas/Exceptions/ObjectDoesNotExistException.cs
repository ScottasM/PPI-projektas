using System;
namespace PPI_projektas.Exceptions;

public class ObjectDoesNotExistException : Exception
{
    public ObjectDoesNotExistException()
    {
    }

    public ObjectDoesNotExistException(string message)
        : base(message)
    {
    }

    public ObjectDoesNotExistException(string message, Exception inner)
        : base(message, inner)
    {
    }

    public ObjectDoesNotExistException(Guid id)
        : base($"Object with ID {id} does not exist!")
    {
    }
}