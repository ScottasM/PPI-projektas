using System.Collections.Concurrent;
using PPI_projektas.objects.abstractions;

namespace PPI_projektas.objects;

public class Group : Entity, IComparable<Group>
{

    public string Name { get; set; }

    public User Owner { get; set; }
    
    public List<User> Members { get; set; } = new List<User>();
    public List<Note> Notes { get; set; } = new();
    
    private object listLock = new();


    public Group () {} // For deserialization
    
    public Group(string name, User owner, List<User> members, bool createGuid = true) : base(createGuid)
    {
        Name = name;
        Owner = owner;
        //Members = members;
    }
    
    public int CompareTo(Group anotherGroup)
    {
        return String.Compare(Name, anotherGroup.Name);
    }

    
    public void AddNote(Note note)
    {
        lock (listLock)
        {
            Notes.Add(note);
        }
    }

    public void RemoveNote(Note note)
    {
        lock (listLock)
        {
            Notes.Remove(note);
        }
    }
    
    public void AddUser(User member)
    {
        lock (listLock)
        {
            Members.Add(member);
        }
    }

    public void RemoveUser(User member)
    {
        lock (listLock)
        {
            Members.Remove(member);
        }
    }
}