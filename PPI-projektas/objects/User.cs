using PPI_projektas.objects.abstractions;
using System.Text.Json.Serialization;
using PPI_projektas.Utils;

namespace PPI_projektas.objects;

public class User : Entity
{
    [JsonInclude] public string Username;
    private readonly string _password;
    private readonly string _email;


    [JsonIgnore] public List<Note> CreatedNotes = new();
    [JsonInclude] public List<Guid> CreatedNotesGuids;

    [JsonIgnore] public List<Note> FavoriteNotes = new();
    [JsonInclude] public List<Guid> FavoriteNotesGuids;
    
    [JsonIgnore] public List<Group> Groups = new();
    [JsonInclude] public List<Guid> GroupsGuids;

    public User () {} // For deserialization
    
    public User(string name, string password, bool createGuid = true) : base(createGuid)
    {
        Username = name;
        _password = password;
        FavoriteNotesGuids = new List<Guid>();
        CreatedNotesGuids = new List<Guid>();
        GroupsGuids = new List<Guid>();
    }
    
    public User(string name, string password, string email, bool createGuid = true) : this(name, password, createGuid)
    {
        _email = email;
    }

    public string GetUsername() => Username;
    public void SetUsername(string name) => Username = name;

    public string GetPassword() => _password;

    public void LoadCreatedNotes()
    {
        foreach (var id in CreatedNotesGuids)
        {
            var note = DataHandler.Instance.AllNotes.Find(note => note.Id == id);
            if (note != null) CreatedNotes.Add(note);
            else CreatedNotesGuids.Remove(id);
        }
    }
    
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
    
    public void LoadFavoriteNotes()
    {
        foreach (var id in FavoriteNotesGuids)
        {
            var note = DataHandler.Instance.AllNotes.Find(note => note.Id == id);
            if (note != null) FavoriteNotes.Add(note);
            else FavoriteNotesGuids.Remove(id);
        }
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

    public void LoadGroups()
    {
        foreach (var id in GroupsGuids)
        {
            var group = DataHandler.Instance.AllGroups.Find(group => group.Id == id);
            if (group != null) Groups.Add(group);
            else GroupsGuids.Remove(id);
        }
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
