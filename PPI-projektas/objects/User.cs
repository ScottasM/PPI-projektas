using PPI_projektas.objects.abstractions;
using System.Text.Json.Serialization;
using PPI_projektas.Utils;
using System.ComponentModel.DataAnnotations.Schema;

namespace PPI_projektas.objects;

public class User : Entity
{
    [JsonInclude] public string Username;
    private readonly string _password;
    private readonly string _email;


    public List<Note> CreatedNotes = new();
    public List<Note> FavoriteNotes = new();
    public List<Group> Groups = new();


    public User () {} // For deserialization
    
    public User(string name, string password, bool createGuid = true) : base(createGuid)
    {
        Username = name;
        _password = password;

    }
    
    public User(string name, string password, string email, bool createGuid = true) : this(name, password, createGuid)
    {
        _email = email;
    }

    public string GetUsername() => Username;
    public void SetUsername(string name) => Username = name;

    public string GetPassword() => _password;

    
    public void AddCreatedNote(Note note)
    {
        CreatedNotes.Add(note);
    }
    public void RemoveCreatedNote(Note note)
    {
        CreatedNotes.Remove(note);
    }
    
    public void AddFavoriteNote(Note note)
    {
        FavoriteNotes.Add(note);
    }
    public void RemoveFavoriteNote(Note note)
    {
        FavoriteNotes.Remove(note);
    }
    
    public void AddGroup(Group group)
    {
        Groups.Add(group);
    }

    public void RemoveGroup(Group group)
    {
        Groups.Remove(group);
    }
}
