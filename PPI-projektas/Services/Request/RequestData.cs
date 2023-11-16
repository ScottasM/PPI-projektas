using PPI_projektas.objects;

namespace PPI_projektas.Services.Request;

public class RequestData
{
    public Guid UserId { get; set; }
    
    public Guid GroupId { get; set; }
    
    public Tag[]? SearchTags { get; set; }
    
    public SearchType? SearchType { get; set; }
    
    public NoteData? Note { get; set; }

    public RequestData()
    {
        
    }
}

public enum SearchType
{
    Any,
    All
}

public class NoteData
{
    public string Name { get; set; }
        
    public List<Tag> Tags { get; set; }
        
    public string Text { get; set; }

    public NoteData(Guid noteId, string name)
    {
        NoteId = noteId;
        Name = name;
    }

    public NoteData(Guid noteId, string name, List<Tag> tags, string text)
    {
        NoteId = noteId;
        Name = name;
        Tags = tags;
        Text = text;
    }
}