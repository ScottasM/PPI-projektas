using PPI_projektas.objects;

namespace PPI_projektas.Services.Response;

public interface INoteDataFactory
{
    NoteData Create(Guid id, string name, List<EntityStrings> tags, string text);
}

public class NoteDataFactory : INoteDataFactory
{
    public NoteData Create(Guid id, string name, List<EntityStrings> tags, string text)
    {
        return new NoteData(id, name, tags, text);
    }
}

public struct NoteData
{
    public Guid Id { get; set; }
    public string Name { get; set; }
        
    public List<EntityStrings> Tags { get; set; }

    public string Text { get; set; }

    public NoteData(Guid id, string name, List<EntityStrings> tags, string text)
    {
        Id = id;
        Name = name;
        Tags = tags;
        Text = text;
    }
}