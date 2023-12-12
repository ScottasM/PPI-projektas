using PPI_projektas.objects;

namespace PPI_projektas.Services.Response;

public interface IOpenedNoteDataFactory
{
    OpenedNoteData Create(string name, List<EntityString> tags, string text);
}

public class OpenedNoteDataFactory : IOpenedNoteDataFactory
{
    public OpenedNoteData Create(string name, List<EntityString> tags, string text)
    {
        return new OpenedNoteData(name, tags.Select(tag => tag.Text), text);
    }
}

public struct OpenedNoteData
{
    public string Name { get; set; }
        

    public List<EntityString> Tags { get; set; }

    public string Text { get; set; }

    public OpenedNoteData(string name, List<EntityString> tags, string text)
    {
        Name = name;
        Tags = tags;
        Text = text;
    }
}