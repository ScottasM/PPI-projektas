using PPI_projektas.objects.abstractions;

public class User : Entity
{
    private string username;
    private readonly string password;
    private readonly string email;

    public List<Note> createdNotes;
    public List<Note> favoriteNotes;

    public User(string name, string password, string email) : base()
    {
        this.username = name;
        this.password = password;
        this.email = email;
        this.createdNotes = new List<Note>();
        this.favoriteNotes = new List<Note>();
    }

    public string getUsername() => username;
    public void setUsername(string name) => username = name;

    public void addCreatedNote(Note note) => createdNotes.Add(note);
    public void removeCreatedNote(Note note) => createdNotes.Remove(note);

    public void addFavoriteNote(Note note) => favoriteNotes.Add(note);
    public void removeFavoriteNote(Note note) => favoriteNotes.Remove(note);
}
