using PPI_projektas.Exceptions;
using PPI_projektas.objects;
using PPI_projektas.Utils;

namespace PPI_projektas.Services;

public class NoteService
{
    public List<NoteData> GetNotes()
    {
        return DataHandler.Instance.AllNotes
            .Select(note => new NoteData(note.Name, note.Id))
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
    
    public void UpdateNote(Guid noteId, Guid userId, string name, List<string> tags, string text)
    {
        var note = DataHandler.Instance.AllNotes
            .Find(note => note.Id == noteId);
        
        if (note == null) throw new ObjectDoesNotExistException(noteId);
        if (note.AuthorId != userId) throw new UnauthorizedAccessException();
        
        note.Name = name;
        note.Tags = tags;
        note.Text = text;
    }

    public void DeleteNote(Guid noteId, Guid authorId)
    {
        var note = DataHandler.Instance.AllNotes
            .Find(note => note.Id == noteId);
        
        if (note == null) throw new ObjectDoesNotExistException();
        if (note.AuthorId != authorId) throw new UnauthorizedAccessException();
        
        DataHandler.Delete(note);
    }

    public struct NoteData
    {
        public string Name { get; set; }

        public Guid Id { get; set; }

        public NoteData(string name, Guid id)
        {
            Name = name;
            Id = id;
        }
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