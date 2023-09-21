using PPI_projektas.objects.abstractions;

namespace PPI_projektas.objects;

public class User : Entity
{
    private string _username;
    private readonly string _password;
    private readonly string _email;

    public List<Note> CreatedNotes;
    public List<Note> FavoriteNotes;

    public User(string name, string password, string email)
    {
        _username = name;
        _password = password;
        _email = email;
        CreatedNotes = new List<Note>();
        FavoriteNotes = new List<Note>();
    }

    public string GetUsername() => _username;
    public void SetUsername(string name) => _username = name;

    public void AddCreatedNote(Note note) => CreatedNotes.Add(note);
    public void RemoveCreatedNote(Note note) => CreatedNotes.Remove(note);

    public void AddFavoriteNote(Note note) => FavoriteNotes.Add(note);
    public void RemoveFavoriteNote(Note note) => FavoriteNotes.Remove(note);
}