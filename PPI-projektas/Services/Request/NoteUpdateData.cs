namespace PPI_projektas.Services.Request;

public class NoteUpdateData
{
    public Guid UserId { get; set; }
    
    public string Name { get; set; }
        
    public IEnumerable<string> Tags { get; set; }
        
    public string Text { get; set; }

    public NoteUpdateData(Guid userId, string name, IEnumerable<string> tags, string text)
    {
        UserId = userId;
        Name = name;
        Tags = tags;
        Text = text;
    }
}