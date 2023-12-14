namespace PPI_projektas.Services.Request;

public class NoteCreationData
{
    public Guid AuthorId { get; set; }
    
    public Guid GroupId { get; set; }

    public NoteCreationData(Guid authorId, Guid groupId)
    {
        AuthorId = authorId;
        GroupId = groupId;
    }
}