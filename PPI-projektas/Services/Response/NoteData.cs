using PPI_projektas.objects;

namespace PPI_projektas.Services.Response;

public interface INoteDataFactory
{
    NoteData Create(Guid id, string name, List<string> tags, string text, bool canEditPrivileges, bool canEditNote);
}

public class NoteDataFactory : INoteDataFactory
{
    public NoteData Create(Guid id, string name, List<string> tags, string text, bool canEditPrivileges, bool canEditNote)
    {
        return new NoteData(id, name, tags, text, canEditPrivileges, canEditNote);
    }
}

public struct NoteData
{
    public Guid Id { get; set; }
    
    public string Name { get; set; }
    
    public List<string> Tags { get; set; }

    public string Text { get; set; }
    
    public bool CanEditPrivileges { get; set; }
    
    public bool CanEditNote { get; set; }

    public NoteData(Guid id, string name, List<string> tags, string text, bool canEditPrivileges, bool canEditNote)
    {
        Id = id;
        Name = name;
        Tags = tags;
        Text = text;
        CanEditPrivileges = canEditPrivileges;
        CanEditNote = canEditNote;
    }
}