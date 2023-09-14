namespace PPI_projektas;

public class Group
{
    public readonly Guid Id;
    public string Name { get; set; }
    /*
    public User owner { get; set; }
    public List<User> members { get; }
    */
    public List<Note> notes { get; }

    public Group(string name/*, User owner*/)
    {
        Id = new Guid();
        Name = name;
        notes = new List<Note>();
        // this.owner = owner;
        // members = new List<User>();
    }
    /*
    public Group(string name, User owner, List<User> members) : this(name, owner)
    {
        this.members = members;
    }
    */

    /*
    public void CreateNote(User author)
    {
        Note newNote = new Note(author);
        notes.Add(newNote);
    }
    */

    public void AddNote(Note note) => notes.Add(note);

    public void RemoveNote(Note note) => notes.Remove(note);
    
    /*
    public void AddUser(Note note) => members.Add(note);

    public void RemoveUser(Note note) => members.Remove(note);
    */
}