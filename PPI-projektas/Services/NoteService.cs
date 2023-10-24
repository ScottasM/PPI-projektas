using PPI_projektas.Exceptions;
using PPI_projektas.Services.Interfaces;
using PPI_projektas.Services.Response;
using PPI_projektas.Utils;

namespace PPI_projektas.Services;

public class NoteService : INoteService
{
    private readonly IObjectDataItemFactory _objectDataItemFactory;
    private readonly IOpenedNoteDataFactory _openedNoteData;

    public NoteService(IObjectDataItemFactory objectDataItemFactory, IOpenedNoteDataFactory openedNoteData)
    {
        _objectDataItemFactory = objectDataItemFactory;
        _openedNoteData = openedNoteData;
    }
    
    public List<ObjectDataItem> GetNotes()
    {
        return DataHandler.Instance.AllNotes
            .Select(note => _objectDataItemFactory.Create(note.Id, note.Name))
            .ToList();
    }

    public OpenedNoteData GetNote(Guid id)
    {
        var note = DataHandler.Instance.AllNotes
            .Find(note => note.Id == id);
        if (note == null) throw new ObjectDoesNotExistException(id);
        
        return _openedNoteData.Create(note.Name, note.Tags, note.Text);
    }

    public void UpdateNote(Guid noteId, Guid authorId, string name, List<string> tags, string text)
    {
        var note = DataHandler.Instance.AllNotes
            .Find(note => note.Id == noteId);
        if (note == null) throw new ObjectDoesNotExistException(noteId);
        if (note.AuthorGuid != authorId) throw new UnauthorizedAccessException("USER-NOT-AUTHOR");
        
        note.Name = name;
        note.Tags = tags;
        note.Text = text;
    }
}