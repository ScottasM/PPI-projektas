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

    public OpenedNoteData GetNote(Guid id)
    {
        var note = DataHandler.Instance.AllNotes
            .Find(note => note.Id == id);
        if (note == null) throw new ObjectDoesNotExistException(id);
        
        return new OpenedNoteData(note.Name, note.Tags, note.Text);
    }

    public void UpdateNote(Guid id, string name, List<string> tags, string text)
    {
        var note = DataHandler.Instance.AllNotes
            .Find(note => note.Id == id);
        if (note == null) throw new ObjectDoesNotExistException(id);
        
        note.Name = name;
        note.Tags = tags;
        note.Text = text;
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