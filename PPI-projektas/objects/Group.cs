using PPI_projektas.objects.abstractions;
using System.Text.Json.Serialization;
using PPI_projektas.Utils;
using System.ComponentModel.DataAnnotations.Schema;

namespace PPI_projektas.objects;

public class Group : Entity, IComparable<Group>
{

    public string Name { get; set; }

    public User Owner { get; set; }
    public Guid OwnerGuid;

    public List<User> Members { get; } = new();
    public List<Note> Notes { get; } = new();


    public Group () {} // For deserialization
    
    public Group(string name, User owner, bool createGUID = true) : base(createGUID)
    {
        Name = name;
        Owner = owner;
        OwnerGuid = owner.Id;
    }
    
    public int CompareTo(Group anotherGroup)
    {
        return String.Compare(Name, anotherGroup.Name);
    }

    public Group(string name, User owner, List<User> members) : this(name, owner)
    {
        Members = members;

    }
    

    
    public void CreateNote(Guid authorId)
    {
        var newNote = new Note(authorId);
        Notes.Add(newNote);
    }
    
    public void AddNote(Note note)
    {
        Notes.Add(note);

    }

    public void RemoveNote(Note note)
    {
        Notes.Remove(note);
    }
    


    public void AddUser(User member)
    {
        Members.Add(member);
    }

    public void RemoveUser(User member)
    {
        Members.Remove(member);
    }
    
}