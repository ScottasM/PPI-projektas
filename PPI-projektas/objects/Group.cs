namespace PPI_projektas;

using PPI_projektas.objects.abstractions;

public class Group : Entity
{

    public string Name { get; set; }
    
    public User Owner { get; set; }

    public List<User> Members { get; }
    
    public List<Note> Notes { get; }


    public Group(string name, User owner) : base()
    {
        Name = name;
        Notes = new List<Note>();
        Owner = owner;
        Members = new List<User>();
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
    

    public void AddNote(Note note) => Notes.Add(note);

    public void RemoveNote(Note note) => Notes.Remove(note);
    
    public void AddUser(User member) => Members.Add(member);

    public void RemoveUser(User member) => Members.Remove(member);
    
}