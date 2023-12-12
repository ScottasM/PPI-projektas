using System.Collections.Concurrent;
using PPI_projektas.Exceptions;
using PPI_projektas.Services.Interfaces;
using PPI_projektas.objects.Factories;
using PPI_projektas.objects;
using PPI_projektas.Services.Response;
using PPI_projektas.Utils;
using PPI_projektas.objects;

namespace PPI_projektas.Services;

public class NoteService : INoteService
{
    private readonly IObjectDataItemFactory _objectDataItemFactory;
    private readonly IOpenedNoteDataFactory _openedNoteDataFactory;
    private readonly INoteFactory _noteFactory;

    public NoteService(IObjectDataItemFactory objectDataItemFactory, IOpenedNoteDataFactory openedNoteDataFactory, INoteFactory noteFactory)
    {
        _objectDataItemFactory = objectDataItemFactory;
        _openedNoteDataFactory = openedNoteDataFactory;
        _noteFactory = noteFactory;
    }

    public IEnumerable<ObjectDataItem> GetNotes(Guid userId, SearchType searchType, string? tagFilter, string? nameFilter, Guid? groupId)
    {
        var userGroupIds = groupId == null
            ? DataHandler.FindObjectById(userId, DataHandler.Instance.AllUsers)
                .Groups.Select(group => group.Id)
            : null;
        var tags = ConvertToArray(tagFilter);
        
        return DataHandler.Instance.AllNotes.Values
            .If(groupId == null, query =>
                query.Where(note => userGroupIds.Contains(note.Group.Id)))
            .If(groupId != null, query =>
                query.Where(note => note.Group.Id == groupId))
            .If(tags.Any(), query =>
                {
                    return searchType switch
                    {
                        SearchType.All => query.Where(note => note.ContainsAll(tags)),
                        SearchType.Any => query.Where(note => note.ContainsAny(tags)),
                        _ => throw new ArgumentOutOfRangeException(nameof(searchType), searchType, null)
                    };
                })
            .If(!string.IsNullOrEmpty(nameFilter), query =>
                query.Where(note => note.Name.Contains(nameFilter)))
            .Select(note => _objectDataItemFactory.Create(note.Id, note.Name));
    }
    
    public OpenedNoteData GetNote(Guid userId, Guid noteId)
    {
        var userGroupIds = DataHandler.FindObjectById(userId, DataHandler.Instance.AllUsers)
            .Groups.Select(group => group.Id);
        
        var note = DataHandler.Instance.AllNotes.Values
            .Where(note => userGroupIds.Contains(note.Group.Id))
            .FirstOrDefault(note => note.Id == noteId);

        if (note == null) throw new ObjectDoesNotExistException(noteId);
        
        return _openedNoteDataFactory.Create(note.Name, note.Tags, note.Text);
    }

    public Guid CreateNote(Guid authorId, Guid groupId)
    {
        var author = DataHandler.FindObjectById(authorId, DataHandler.Instance.AllUsers);
        var group = DataHandler.FindObjectById(groupId, DataHandler.Instance.AllGroups);

        if (author == null) throw new ObjectDoesNotExistException(authorId);
        if (group == null) throw new ObjectDoesNotExistException(groupId);
        
        var note = _noteFactory.Create(authorId, groupId);
        
        DataHandler.Create(note);
        author.AddCreatedNote(note);
        group.AddNote(note);

        return note.Id;
    }
    
    public void UpdateNote(Guid userId, Guid noteId, string name, IEnumerable<string> tags, string text)
    {
        var note = DataHandler.FindObjectById(noteId, DataHandler.Instance.AllNotes);

        if (note.User.Id != userId) throw new UnauthorizedAccessException();
        
        note.Name = name;
        if (note.Tags.Count != tags.Count()) note.Tags = ReplaceTags(note.Tags, tags);
        note.Text = text;
    }

    public void DeleteNote(Guid userId, Guid noteId)
    {
        var note = DataHandler.FindObjectById(noteId, DataHandler.Instance.AllNotes);
        var user = DataHandler.FindObjectById(userId, DataHandler.Instance.AllUsers);

        var group = DataHandler.Instance.AllGroups.Values.FirstOrDefault(group => group.Notes.Contains(note));

        if (group == null) throw new ObjectDoesNotExistException();
        if (note.User.Id != userId) throw new UnauthorizedAccessException();
        
        group.RemoveNote(note);
        user.RemoveCreatedNote(note);
        DataHandler.Delete(note);
    }

    private IEnumerable<string> ConvertToArray(string tagFilter)
    {
        if (string.IsNullOrEmpty(tagFilter)) return Enumerable.Empty<string>();
        
        var separators = new char[] {',', ';'};
        return tagFilter.Split(separators, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
    }
    
    private List<EntityString> ReplaceTags(List<EntityString> tags, IEnumerable<string> incomingTags)
    {
        foreach (var tag in tags)
        {
            if (!incomingTags.Contains(tag.Value))
                
        }
    }
}

public enum SearchType
{
    All,
    Any
}
