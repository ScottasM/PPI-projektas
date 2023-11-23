﻿using PPI_projektas.Exceptions;
using PPI_projektas.Services.Interfaces;
using PPI_projektas.objects.Factories;
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

    public List<NoteData> GetNotes(Guid groupId)
    {
        var group = DataHandler.FindObjectById(groupId, DataHandler.Instance.AllGroups);

        return group.Notes.Select(note => _noteDataFactory.Create(note.Id, note.Name, note.Tags, note.Text)).ToList();
    }

    public NoteData GetNote(Guid noteId)
    {
        var note = DataHandler.FindObjectById(noteId, DataHandler.Instance.AllNotes);
        
        return _noteDataFactory.Create(note.Id, note.Name, note.Tags, note.Text);
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
    
    public void UpdateNote(Guid noteId, Guid userId, string name, List<string> tags, string text)
    {
        var note = DataHandler.FindObjectById(noteId, DataHandler.Instance.AllNotes);
        if (note.AuthorId != userId) throw new UnauthorizedAccessException();
        
        note.Name = name;
        note.Tags = tags;
        note.Text = text;
    }

    public void DeleteNote(Guid noteId, Guid userId)
    {
        var note = DataHandler.FindObjectById(noteId, DataHandler.Instance.AllNotes);
        var user = DataHandler.FindObjectById(userId, DataHandler.Instance.AllUsers);
        
        var group = DataHandler.Instance.AllGroups.FirstOrDefault(group => group.Notes.Contains(note));
        if (group == null) throw new ObjectDoesNotExistException();
        
        if (note.AuthorId != userId) throw new UnauthorizedAccessException();
        
        group.RemoveNote(note);
        user.RemoveCreatedNote(note);
        DataHandler.Delete(note);
    }
}