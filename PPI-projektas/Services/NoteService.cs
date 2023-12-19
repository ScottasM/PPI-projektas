using PPI_projektas.Exceptions;
using PPI_projektas.Services.Interfaces;
using PPI_projektas.objects.Factories;
using PPI_projektas.objects;
using PPI_projektas.Services.Response;
using PPI_projektas.Utils;

namespace PPI_projektas.Services;

public class NoteService : INoteService
{
    private readonly IObjectDataItemFactory _objectDataItemFactory;
    private readonly INoteDataFactory _noteDataFactory;
    private readonly INoteFactory _noteFactory;

    public NoteService(IObjectDataItemFactory objectDataItemFactory, INoteDataFactory noteDataFactory, INoteFactory noteFactory)
    {
        _objectDataItemFactory = objectDataItemFactory;
        _noteDataFactory = noteDataFactory;
        _noteFactory = noteFactory;
    }

    public IEnumerable<NoteData> GetNotes(Guid userId, SearchType searchType, string? tagFilter, string? nameFilter, Guid? groupId)
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
            .Select(note => _noteDataFactory.Create(note.Id, note.Name, note.Tags.Select(tag => tag.Value).ToList(), note.Text));
    }
    
    public NoteData GetNote(Guid userId, Guid noteId)
    {
        var userGroupIds = DataHandler.FindObjectById(userId, DataHandler.Instance.AllUsers)
            .Groups.Select(group => group.Id);
        
        var note = DataHandler.Instance.AllNotes.Values
            .Where(note => userGroupIds.Contains(note.Group.Id))
            .FirstOrDefault(note => note.Id == noteId);

        if (note == null) throw new ObjectDoesNotExistException(noteId);

        var tags = note.Tags?.Any() == null ? new List<Tag>() : note.Tags;
        
        return _noteDataFactory.Create(note.Id, note.Name, tags.Select(tag => tag.Value).ToList(), note.Text);
    }

    public Guid CreateNote(Guid userId, Guid groupId)
    {

        var author = DataHandler.FindObjectById(userId, DataHandler.Instance.AllUsers);
        var group = DataHandler.FindObjectById(groupId, DataHandler.Instance.AllGroups);

        if (author == null) throw new ObjectDoesNotExistException(userId);
        if (group == null) throw new ObjectDoesNotExistException(groupId);
        
        var note = _noteFactory.Create(userId, groupId);
        
        DataHandler.Create(note);
        author.AddCreatedNote(note);
        group.AddNote(note);

        return note.Id;
    }
    
    public void UpdateNote(Guid userId, Guid noteId, string name, List<string> tags, string text)
    {
        var note = DataHandler.FindObjectById(noteId, DataHandler.Instance.AllNotes);
        if (note == null) throw new ObjectDoesNotExistException(noteId);
        
        if (note.Author.Id != userId
            && note.Group.Owner.Id != userId
            && !note.Editors.Select(user => user.Id).Contains(userId)
            && !note.Group.Administrators.Select(user => userId).Contains(userId))
            throw new UnauthorizedAccessException();
        
        note.Name = name;
        note.Tags = tags.Select(tag => new Tag(tag)).ToList();
        note.Text = text;

        DataHandler.Instance.SaveChanges();
    }

    public List<ObjectDataItem> GetPrivileges(Guid noteId)
    {
        var note = DataHandler.FindObjectById(noteId, DataHandler.Instance.AllNotes);
        if (note == null) throw new ObjectDoesNotExistException(noteId);

        return note.Editors
            .Select(editor => _objectDataItemFactory.Create(editor.Id, editor.GetUsername()))
            .ToList();
    }

    public void UpdatePrivileges(Guid userId, Guid noteId, List<Guid> newEditorIds)
    {
        var note = DataHandler.FindObjectById(noteId, DataHandler.Instance.AllNotes);
        if (note == null) throw new ObjectDoesNotExistException(noteId);

        if (note.Author.Id != userId
            && note.Group.Owner.Id != userId
            && note.Group.Administrators.Select(user => user.Id).Contains(userId))
            throw new UnauthorizedAccessException();

        note.Editors = newEditorIds
            .Select(editorId => DataHandler.FindObjectById(editorId, DataHandler.Instance.AllUsers))
            .ToList();
        
        DataHandler.Instance.SaveChanges();
    }
    

    public List<ObjectDataItem> GetPrivileges(Guid noteId)
    {
        var note = DataHandler.FindObjectById(noteId, DataHandler.Instance.AllNotes);
        if (note == null) throw new ObjectDoesNotExistException(noteId);

        return note.Editors
            .Select(editor => _objectDataItemFactory.Create(editor.Id, editor.GetUsername()))
            .ToList();
    }

    public void UpdatePrivileges(Guid userId, Guid noteId, List<Guid> newEditorIds)
    {
        var note = DataHandler.FindObjectById(noteId, DataHandler.Instance.AllNotes);
        if (note == null) throw new ObjectDoesNotExistException(noteId);

        if (note.Author.Id != userId
            && note.Group.Owner.Id != userId
            && note.Group.Administrators.Select(user => user.Id).Contains(userId))
            throw new UnauthorizedAccessException();

        note.Editors = newEditorIds
            .Select(editorId => DataHandler.FindObjectById(editorId, DataHandler.Instance.AllUsers))
            .ToList();
        
        DataHandler.Instance.SaveChanges();
    }
    

    public void DeleteNote(Guid userId, Guid noteId)
    {
        var note = DataHandler.FindObjectById(noteId, DataHandler.Instance.AllNotes);
        var user = DataHandler.FindObjectById(userId, DataHandler.Instance.AllUsers);

        var group = DataHandler.Instance.AllGroups.Values.FirstOrDefault(group => group.Notes.Contains(note));

        if (group == null) throw new ObjectDoesNotExistException();

        if (note.UserId != userId
            && note.Group.Owner.Id != userId
            && !note.Editors.Select(editor => editor.Id).Contains(userId)
            && !note.Group.Administrators.Select(administrator => administrator.Id).Contains(userId))
            throw new UnauthorizedAccessException();
        
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

    public List<string> SearchTags(Guid groupId, string search)
    {
        var group = DataHandler.FindObjectById(groupId, DataHandler.Instance.AllGroups);

        var tags = new List<string>();

        foreach (var note in group.Notes)
        {
            if (!note.Tags?.Any() ?? false)
                continue;
            var uniqueValues = note.Tags
                .Where(tag => tag.Value.Contains(search))
                .Select(tag => tag.Value)
                .Except(tags)
                .ToList();
            
            tags.AddRange(uniqueValues);
        }

        return tags;
    }
}

public enum SearchType
{
    All,
    Any
}