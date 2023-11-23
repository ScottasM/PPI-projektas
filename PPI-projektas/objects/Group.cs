using System.Collections.Concurrent;
using PPI_projektas.objects.abstractions;

namespace PPI_projektas.objects;

public class Group : Entity, IComparable<Group>
{

    public string Name { get; set; }

    public User Owner { get; set; }
    public Guid OwnerGuid;
    
    public ConcurrentDictionary<Guid, User> Members { get; } = new();
    public ConcurrentDictionary<Guid, Note> Notes { get; } = new();


    public Group () {} // For deserialization
    
    public Group(string name, User owner, List<User> members, bool createGuid = true) : base(createGuid)
    {
        Name = name;
        Owner = owner;
        OwnerGuid = owner.Id;
        members.ForEach(member => Members.TryAdd(member.Id, member));
    }
    
    public int CompareTo(Group anotherGroup)
    {
        return String.Compare(Name, anotherGroup.Name);
    }

    
    public void AddNote(Note note)
    {
        Notes.TryAdd(note.Id, note);

    }

    public void RemoveNote(Note note)
    {
        Notes.TryRemove(note.Id, out _);
    }
    
    public void AddUser(User member)
    {
        Members.TryAdd(member.Id, member);
    }

    public void RemoveUser(User member)
    {
        Members.TryRemove(member.Id, out _);
    }
}