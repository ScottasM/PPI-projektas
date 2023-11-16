using PPI_projektas.objects.abstractions;
using System.Text.Json.Serialization;
using PPI_projektas.Utils;

namespace PPI_projektas.objects;

public class Group : Entity, IComparable<Group>
{

    public string Name { get; set; }

    [JsonIgnore] public User Owner { get; set; }
    [JsonInclude] public Guid OwnerGuid;

    [JsonIgnore] public List<User> Members { get; } = new();
    [JsonInclude] public List<Guid> MembersGuid;

    [JsonIgnore] public List<Note> Notes { get; } = new();
    [JsonInclude] public List<Guid> NotesGuid;

    public Group () {} // For deserialization
    
    public Group(string name, User owner, List<User> members, bool createGuid = true) : base(createGuid)
    {
        Name = name;
        Owner = owner;
        OwnerGuid = owner.Id;

        NotesGuid = new List<Guid>();
        
        Members = members;
        MembersGuid = new List<Guid>();
        foreach (var member in members) 
            MembersGuid.Add(member.Id);
    }
    
    public int CompareTo(Group anotherGroup)
    {
        return String.Compare(Name, anotherGroup.Name);
    }
    
    public void LoadNotes()
    {
        foreach (var id in NotesGuid)
        {
            var note = DataHandler.Instance.AllNotes.Find(note => note.Id == id);
            if (note != null) Notes.Add(note);
            else NotesGuid.Remove(id);
        }
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
    
    public void LoadMembers()
    {
        foreach (var id in MembersGuid)
        {
            var user = DataHandler.Instance.AllUsers.Find(user => user.Id == id);
            if (user != null) Members.Add(user);
            else MembersGuid.Remove(id);
        }
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