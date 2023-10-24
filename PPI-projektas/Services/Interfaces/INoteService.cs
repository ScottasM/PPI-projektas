using PPI_projektas.Services.Response;

namespace PPI_projektas.Services.Interfaces;

public interface INoteService
{
    public List<ObjectDataItem> GetNotes();
    public OpenedNoteData GetNote(Guid id);
    public void UpdateNote(Guid noteId, Guid authorId, string name, List<string> tags, string text);
}