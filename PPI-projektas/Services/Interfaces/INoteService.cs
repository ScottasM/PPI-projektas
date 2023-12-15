using PPI_projektas.objects;
using PPI_projektas.Services.Response;

namespace PPI_projektas.Services.Interfaces;

public interface INoteService
{
    public IEnumerable<NoteData> GetNotes(Guid userId, SearchType searchType, string? tagFilter, string? nameFilter, Guid? groupId);
    public NoteData GetNote(Guid userId, Guid noteId);
    public Guid CreateNote(Guid userId, Guid groupId);
    public void UpdateNote(Guid userId, Guid noteId, string name, List<string> tags, string text);
    public void DeleteNote(Guid userId, Guid noteId);
    List<string> SearchTags(Guid groupId, string search);
}