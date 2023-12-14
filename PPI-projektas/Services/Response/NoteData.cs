using PPI_projektas.objects;

namespace PPI_projektas.Services.Response;

public interface IOpenedNoteDataFactory
{
    NoteData Create(Guid id, string name, List<Tag> tags, string text);
}

public class OpenedNoteDataFactory : IOpenedNoteDataFactory
{
    public NoteData Create(Guid id, string name, List<Tag> tags, string text)
    {
        return new NoteData(id, name, tags.Select(tag => tag.Value).ToList(), text);
    }
}

public struct NoteData
{
    public Guid Id { get; set; }
    
    public string Name { get; set; }
    
    public List<string> Tags { get; set; }

    public string Text { get; set; }

    public NoteData(Guid id, string name, List<string> tags, string text)
    {
        Id = id;
        Name = name;
        Tags = tags;
        Text = text;
    }
}