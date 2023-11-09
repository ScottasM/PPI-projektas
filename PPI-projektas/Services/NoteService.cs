using PPI_projektas.Exceptions;
using PPI_projektas.objects;
using PPI_projektas.Services.Response;
using PPI_projektas.Utils;

namespace PPI_projektas.Services;

public class NoteService
{
    public List<ObjectDataItem> GetNotes()
    {
        return DataHandler.Instance.AllNotes
            .Select(note => new ObjectDataItem(note.Id, note.Name))
            .ToList();
    }

    public OpenedNoteData GetNote(Guid noteId)
    {
        var note = DataHandler.Instance.AllNotes
            .Find(note => note.Id == noteId);
        
        if (note == null) throw new ObjectDoesNotExistException();
        
        return new OpenedNoteData(note.Name, note.Tags, note.Text);
    }

    public Guid CreateNote(Guid authorId)
    {
        var note = new Note(authorId);
        DataHandler.Create(note);

        return note.Id;
    }
    
    public void UpdateNote(Guid noteId, Guid userId, string name, List<EntityStrings> tags, string text)
    {
        var note = DataHandler.Instance.AllNotes
            .Find(note => note.Id == noteId);
        
        if (note == null) throw new ObjectDoesNotExistException();
        if (note.UserId != userId) throw new UnauthorizedAccessException();
        
        note.Name = name;
        note.Tags = tags;
        note.Text = text;
    }

    public void DeleteNote(Guid noteId, Guid userId)
    {
        var note = DataHandler.Instance.AllNotes
            .Find(note => note.Id == noteId);
        
        if (note == null) throw new ObjectDoesNotExistException();
        if (note.UserId != userId) throw new UnauthorizedAccessException();
        
        DataHandler.Delete(note);
    }

    public struct OpenedNoteData
    {
        public string Name { get; set; }
        
        public List<EntityStrings> Tags { get; set; }

        public string Text { get; set; }

        public OpenedNoteData(string name, List<EntityStrings> tags, string text)
        {
            Name = name;
            Tags = tags;
            Text = text;
        }
    }
}