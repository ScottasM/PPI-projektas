using PPI_projektas.objects;
using PPI_projektas.Services.Response;

namespace PPI_projektas.Services.Interfaces;

public interface INoteService
{
    public IEnumerable<ObjectDataItem> GetNotes(Guid userId, SearchType searchType, string? tagFilter, string? nameFilter, Guid? groupId);
    public NoteData GetNote(Guid userId, Guid noteId);
    public Guid CreateNote(Guid authorId, Guid groupId);
    public void UpdateNote(Guid authorId, Guid noteId, string name, IEnumerable<string> tags, string text);
    public void DeleteNote(Guid userId, Guid noteId);
}