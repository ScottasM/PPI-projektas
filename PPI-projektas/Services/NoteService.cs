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

    public Note GetNote(Guid id)
    {
        var note = DataHandler.Instance.AllNotes
            .Find(note => note.Id == id);
        if (note == null) throw new ObjectDoesNotExistException(id);

        return note;
    }

    public void UpdateNote(Guid id, string name, List<string> tags, string text)
    {
        var note = GetNote(id);
        if (note.Name != name) note.Name = name;
        if (note.Tags != tags) note.Tags = tags;
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
}