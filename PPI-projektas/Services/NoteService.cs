using System.Security.AccessControl;
using Microsoft.AspNetCore.SignalR;
using PPI_projektas.Exceptions;
using PPI_projektas.Services.Interfaces;
using PPI_projektas.objects.Factories;
using PPI_projektas.Services.Response;
using PPI_projektas.Utils;
using PPI_projektas.objects;
using PPI_projektas.Services.Request;

namespace PPI_projektas.Services;

public class NoteService : INoteService
{
    private readonly IObjectDataItemFactory _objectDataItemFactory;
    private readonly IOpenedNoteDataFactory _openedNoteDataFactory;
    private readonly INoteFactory _noteFactory;
    private readonly IPrivilegeDataFactory _privilegeDataFactory;

    public NoteService(IObjectDataItemFactory objectDataItemFactory, IOpenedNoteDataFactory openedNoteDataFactory, INoteFactory noteFactory, IPrivilegeDataFactory privilegeDataFactory)
    {
        _objectDataItemFactory = objectDataItemFactory;
        _openedNoteDataFactory = openedNoteDataFactory;
        _noteFactory = noteFactory;
        _privilegeDataFactory = privilegeDataFactory;
    }

    public List<ObjectDataItem> GetNotes(Guid groupId)
    {
        var group = DataHandler.FindObjectById(groupId, DataHandler.Instance.AllGroups);

        return group.Notes.Select(note => _objectDataItemFactory.Create(note.Id, note.Name)).ToList();
    }

    public OpenedNoteData GetNote(Guid noteId)
    {
        var note = DataHandler.FindObjectById(noteId, DataHandler.Instance.AllNotes);
        
        return _openedNoteDataFactory.Create(note.Name, note.Tags, note.Text);
    }

    public Guid CreateNote(Guid groupId, Guid authorId)
    {
        var group = DataHandler.FindObjectById(groupId, DataHandler.Instance.AllGroups);
        var author = DataHandler.FindObjectById(authorId, DataHandler.Instance.AllUsers);
        var note = _noteFactory.Create(authorId);
        DataHandler.Create(note);
        author.AddCreatedNote(note);
        group.AddNote(note);

        return note.Id;
    }

    public IEnumerable<PrivilegeData> GetPrivileges(Guid noteId)
    {
        var note = DataHandler.FindObjectById(noteId, DataHandler.Instance.AllNotes);

        if (note == null)
            throw new ObjectDoesNotExistException(noteId);
        
        return note.Group.Members
            .Select(member =>
            {
                if (member == note.Author)
                    return _privilegeDataFactory.Create(member.Id, member.Username, Privilege.Author);
                if (note.AdminstratorPrivileges.Contains(member))
                    return _privilegeDataFactory.Create(member.Id, member.Username, Privilege.Administrator);
                if (note.EditingPriveleges.Contains(member))
                    return _privilegeDataFactory.Create(member.Id, member.Username, Privilege.Editor);
                return _privilegeDataFactory.Create(member.Id, member.Username, Privilege.Viewer);
            });
    }
    
    public IEnumerable<UpdatePrivilegeResult> UpdatePrivileges(Guid noteId, Guid userId, IEnumerable<PrivilegeData> updatedPrivileges)
    {
        var note = DataHandler.FindObjectById(noteId, DataHandler.Instance.AllNotes);

        if (note == null)
            throw new ObjectDoesNotExistException(noteId);
        
        if (userId != note.Author.Id && !note.AdminstratorPrivileges.Select(user => user.Id).Contains(userId))
            throw new PrivilegeNotHeldException();

        return updatedPrivileges.Select(privilege =>
            {
                if (privilege.User != note.Author && !note.AdminstratorPrivileges.Contains(privilege.User))
                    return new UpdatePrivilegeResult(
                        "User does not hold the privilege to give or take away privileges.");
                
                switch (privilege.Privilege)
                {
                    case Privilege.Author:
                        return new UpdatePrivilegeResult(
                            "Author privileges cannot be given or taken away.");
                    
                    case Privilege.Administrator:
                        if (userId != note.Author.Id)
                            return new UpdatePrivilegeResult(
                                "Only the note author can give administrator privileges.");
                        
                        note.AdminstratorPrivileges.Add(privilege.User);
                        return new UpdatePrivilegeResult();
                    
                    case Privilege.Editor:
                        if (note.AdminstratorPrivileges.Contains(privilege.User))
                            return new UpdatePrivilegeResult(
                                $"Cannot set {privilege.User.Username}'s privilege to {privilege.Privilege}, because only the note author can take away administrator privileges.");
                        
                        if (note.EditingPriveleges.Contains(privilege.User))
                            return new UpdatePrivilegeResult(
                                $"Cannot set {privilege.User.Username}'s privilege to {privilege.Privilege}, because the user already holds this privilege.");
                        
                        note.EditingPriveleges.Add(privilege.User);
                        return new UpdatePrivilegeResult();
                    
                    case Privilege.Viewer:
                        if (note.AdminstratorPrivileges.Contains(privilege.User))
                            return new UpdatePrivilegeResult(
                                $"Cannot set {privilege.User.Username}'s privilege to {privilege.Privilege}, because only the note author can take away administrator privileges.");
                        
                        if (!note.EditingPriveleges.Contains(privilege.User))
                            return new UpdatePrivilegeResult(
                                $"Cannot set {privilege.User.Username}'s privilege to {privilege.Privilege}, because the user already holds this privilege.");
                        
                        note.EditingPriveleges.Remove(privilege.User);
                        return new UpdatePrivilegeResult();
                    
                    default:
                        throw new ArgumentOutOfRangeException(nameof(privilege.Privilege), privilege.Privilege, null);
                }
            }
        );
    }
    
    public void UpdateNote(Guid noteId, Guid userId, string name, List<EntityStrings> tags, string text)
    {
        var note = DataHandler.FindObjectById(noteId, DataHandler.Instance.AllNotes);
        
        if (note.Author.Id != userId
            && !note.AdminstratorPrivileges.Select(user => user.Id).Contains(userId)
            && !note.EditingPriveleges.Select(user => user.Id).Contains(userId)) throw new UnauthorizedAccessException();

        
        note.Name = name;
        note.Tags = tags;
        note.Text = text;
    }

    public void DeleteNote(Guid noteId, Guid userId)
    {
        var note = DataHandler.FindObjectById(noteId, DataHandler.Instance.AllNotes);
        var user = DataHandler.FindObjectById(userId, DataHandler.Instance.AllUsers);
        
        var group = DataHandler.Instance.AllGroups.Values.FirstOrDefault(group => group.Notes.Contains(note));
        if (group == null) throw new ObjectDoesNotExistException();
        
        if (note.Author.Id != userId
            && !note.AdminstratorPrivileges.Select(user => user.Id).Contains(userId)
            && !note.EditingPriveleges.Select(user => user.Id).Contains(userId)) throw new UnauthorizedAccessException();
        
        group.RemoveNote(note);
        user.RemoveCreatedNote(note);
        DataHandler.Delete(note);
    }
}