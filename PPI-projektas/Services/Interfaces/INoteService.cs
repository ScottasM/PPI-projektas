using PPI_projektas.objects;
using PPI_projektas.Services.Request;
using PPI_projektas.Services.Response;

namespace PPI_projektas.Services.Interfaces;

public interface INoteService
{
    public List<ObjectDataItem> GetNotes(Guid groupId);
    public OpenedNoteData GetNote(Guid id);
    public Guid CreateNote(Guid groupId, Guid authorId);
    public IEnumerable<PrivilegeData> GetPrivileges(Guid noteId);
    public IEnumerable<UpdatePrivilegeResult> UpdatePrivileges(Guid noteId, Guid userId,
        IEnumerable<PrivilegeData> updatedPrivileges);
    public void UpdateNote(Guid noteId, Guid authorId, string name, List<EntityStrings> tags, string text);
    public void DeleteNote(Guid noteId, Guid userId);
}