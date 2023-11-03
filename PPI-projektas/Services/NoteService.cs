﻿using PPI_projektas.Services.Interfaces;
using PPI_projektas.objects.Factories;
using PPI_projektas.Services.Response;
using PPI_projektas.Utils;

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
    
    public List<ObjectDataItem> GetNotes()
    {
        return DataHandler.Instance.AllNotes
            .Select(note => _objectDataItemFactory.Create(note.Id, note.Name))
            .ToList();
    }

    public OpenedNoteData GetNote(Guid noteId)
    {
        var note = DataHandler.FindObjectById(noteId, DataHandler.Instance.AllNotes);
        
        return _openedNoteDataFactory.Create(note.Name, note.Tags, note.Text);
    }

    public Guid CreateNote(Guid groupId, Guid authorId)
    {
        var group = DataHandler.FindObjectById(groupId, DataHandler.Instance.AllGroups);
        var note = _noteFactory.Create(authorId);
        DataHandler.Create(note);
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
        
        if (note.AuthorId != userId) throw new UnauthorizedAccessException();
        
        DataHandler.Delete(note);
    }
}