namespace PPI_projektas.Services.Request;

public class NotePrivilegeUpdateData
{
    public Guid UserId { get; set; }

    public List<Guid> EditorIds { get; set; } = new();

    public NotePrivilegeUpdateData(Guid userId, List<Guid> editorIds)
    {
        UserId = userId;
        EditorIds = editorIds;
    }
}