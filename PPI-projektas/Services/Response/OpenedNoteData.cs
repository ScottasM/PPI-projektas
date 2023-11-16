namespace PPI_projektas.Services.Response;

public interface IOpenedNoteDataFactory
{
    OpenedNoteData Create(string name, List<string> tags, string text);
}

public class OpenedNoteDataFactory : IOpenedNoteDataFactory
{
    public OpenedNoteData Create(string name, List<string> tags, string text)
    {
        return new OpenedNoteData(name, tags, text);
    }
}

public struct OpenedNoteData
{
    public string Name { get; set; }
        
    public List<string> Tags { get; set; }

    public string Text { get; set; }

    public OpenedNoteData(string name, List<string> tags, string text)
    {
        Name = name;
        Tags = tags;
        Text = text;
    }
}