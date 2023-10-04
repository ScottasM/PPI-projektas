using PPI_projektas.objects.abstractions;
using System.Text.Json.Serialization;

namespace PPI_projektas.objects;

public class Group : Entity
{

    public string Name { get; set; }

    [JsonIgnore] public User Owner { get; set; }
    public Guid OwnerGuid;

    [JsonIgnore] public List<User> Members { get; }
    public List<Guid> MembersGuid;

    [JsonIgnore] public List<Note> Notes { get; }
    public List<Guid> NotesGuid;


    public Group(string name, User owner, bool createGUID = false) : base(createGUID)
    {
        Name = name;
        Notes = new List<Note>();
        Owner = owner;
        Members = new List<User>();

        NotesGuid = new List<Guid>();
        MembersGuid = new List<Guid>();
        
    }
    

    public Group(string name, User owner, List<User> members) : this(name, owner)
    {
        Members = members;
    }
    
    
    public void CreateNote(User author)
    {
        var newNote = new Note(author);
        Notes.Add(newNote);
    }


    public void AddNote(Note note)
    {
        Notes.Add(note);
        NotesGuid.Add(note.Id);
    }

    public void RemoveNote(Note note)
    {
        NotesGuid.Remove(note.Id);
        Notes.Remove(note);
    }

    public void AddUser(User member)
    {
        Members.Add(member);
        MembersGuid.Add(member.Id);
    }

    public void RemoveUser(User member)
    {
        MembersGuid.Remove(member.Id);
        Members.Remove(member);
    }
    
}