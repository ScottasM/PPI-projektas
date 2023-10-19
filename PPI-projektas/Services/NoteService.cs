using PPI_projektas.Exceptions;
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

    public OpenedNoteData GetNote(Guid id)
    {
        var note = DataHandler.Instance.AllNotes
            .Find(note => note.Id == id);
        if (note == null) throw new ObjectDoesNotExistException(id);
        
        return new OpenedNoteData(note.Name, note.Tags, note.Text);
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

    public struct OpenedNoteData
    {
        public string Name { get; set; }
        
        public List<string> Tags { get; set; }

        public string Text { get; set; }

        public OpenedNoteData(string name, List<string> tags, string text)
        {
            Name = name;
            Tags = tags;
            Text = text;
        }
    }
}