namespace PPI_projektas.Exceptions;

public class ObjectDoesNotExistException : Exception
{
    public ObjectDoesNotExistException()
    {
    }

    public ObjectDoesNotExistException(string message)
        : base(message)
    {
        LogExceptionDetails(message);
    }

    public ObjectDoesNotExistException(string message, Exception inner)
        : base(message, inner)
    {
    }

    public ObjectDoesNotExistException(Guid id)
        : base($"Object with ID {id} does not exist!")
    {
    }
    
    private void LogExceptionDetails(string message)
    {
        var id = GetObjectIdFromMessage(message);
        var className = GetCallingClassName();

        var logMessage = $"ObjectDoesNotExistException - ID: {id}, Class: {className}, Message: {message}, Date: {DateTime.Now:dd/MM/yyyy}";
        WriteToFile(logMessage);
    }
    
    private void WriteToFile(string logMessage)
    {
        const string logFilePath = "/Exceptions/logs/ObjectDoesNotExistLog.txt";
        
        var directory = Path.GetDirectoryName(logFilePath);
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
        
        using (var fs = new FileStream(logFilePath, FileMode.Append))
        using (var sw = new StreamWriter(fs))
        {
            sw.WriteLine(logMessage);
        }
    }

    private Guid GetObjectIdFromMessage(string message)
    {
        var start = message.IndexOf("ID ") + 3;
        var end = message.IndexOf(" does not exist!");

        if (start >= 0 && end >= 0)
        {
            var idString = message.Substring(start, end - start);
            if (Guid.TryParse(idString, out var id))
            {
                return id;
            }
        }

        return Guid.Empty;
    }

    private string GetCallingClassName()
    {
        var frames = new System.Diagnostics.StackTrace(true).GetFrames();
        if (frames is { Length: > 2 })
        {
            return frames[2].GetMethod().DeclaringType?.Name ?? "UnknownClass";
        }

        return "UnknownClass";
    }
}