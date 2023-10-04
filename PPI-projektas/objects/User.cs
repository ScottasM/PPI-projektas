using PPI_projektas.objects.abstractions;
using System.Text.Json.Serialization;

namespace PPI_projektas.objects;

public class User : Entity
{
    public string _username;
    private readonly string _password;
    private readonly string _email;


    [JsonIgnore] public List<Note> CreatedNotes;
    public List<Guid> CreatedNotesGuids;

    [JsonIgnore] public List<Note> FavoriteNotes;
    public List<Guid> FavoriteNotesGuids;


    [JsonIgnore] public List<Group> Groups;
    public List<Guid> GroupsGuids;

    public User(string name, string password, string email)
    {
        _username = name;
        _password = password;
        _email = email;
        CreatedNotes = new List<Note>();
        FavoriteNotes = new List<Note>();
        FavoriteNotesGuids = new List<Guid>();
        CreatedNotesGuids = new List<Guid>();
        GroupsGuids = new List<Guid>();
    }

    public string GetUsername() => _username;
    public void SetUsername(string name) => _username = name;

    public void AddCreatedNote(Note note)
    {
        CreatedNotes.Add(note);
        CreatedNotesGuids.Add(note.Id);
    }
    public void RemoveCreatedNote(Note note)
    {
        CreatedNotes.Remove(note);
        CreatedNotesGuids.Remove(note.Id);
    }

    public void AddFavoriteNote(Note note)
    {
        FavoriteNotes.Add(note);
        FavoriteNotesGuids.Add(note.Id);
    }
    public void RemoveFavoriteNote(Note note)
    {
        FavoriteNotes.Remove(note);
        FavoriteNotesGuids.Remove(note.Id);
    }

    public void AddGroup(Group group)
    {
        Groups.Add(group);
        GroupsGuids.Add(group.Id);
    }

    public void RemoveGroup(Group group)
    {
        Groups.Remove(group);
        GroupsGuids.Remove(group.Id);
    }
}
