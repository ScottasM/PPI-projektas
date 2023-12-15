using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using PPI_projektas.Exceptions;
using PPI_projektas.objects;
using PPI_projektas.Services;
using PPI_projektas.Services.Interfaces;
using PPI_projektas.Services.Request;
using PPI_projektas.Services.Response;

namespace PPI_projektas.Controllers

{
    [ApiController]
    [Route("api/[controller]")]
    public class NoteController : ControllerBase
    {
        private readonly INoteService _noteService;

        public NoteController(INoteService noteService)
        {
            _noteService = noteService;
        }
        
        [HttpGet("search")]
        public IActionResult Get([FromQuery] Guid userId, [FromQuery] SearchType searchType, [FromQuery] string? tagFilter, [FromQuery] string? nameFilter, [FromQuery] Guid? groupId)
        {
            return Ok(_noteService.GetNotes(userId, searchType, tagFilter, nameFilter, groupId));
        }

        [HttpGet("openNote/{noteId:guid}/{userId:guid}")]
        public IActionResult OpenNote(Guid noteId, Guid userId)
        {
            try
            {
                return Ok(_noteService.GetNote(userId, noteId));
            }
            catch (ObjectDoesNotExistException)
            {
                return NotFound();
            }
        }

        [HttpPost("createNote")]
        public IActionResult CreateNote([FromBody] NoteCreationData data)
        {
            try
            {
                var noteId = _noteService.CreateNote(data.AuthorId, data.GroupId);
                return CreatedAtAction("CreateNote", noteId);
            }
            catch (ObjectDoesNotExistException)
            {
                return NotFound();
            }
        }

        [HttpPost("updateNote/{noteId:guid}")]
        public IActionResult UpdateNote(Guid noteId, [FromBody] NoteData noteData)
        public IActionResult UpdateNote(Guid noteId, [FromBody] NoteData noteData)
        {
            try
            {
                _noteService.UpdateNote(noteData.Id, noteId, noteData.Name, noteData.Tags, noteData.Text);

                return Ok();
            }
            catch (ObjectDoesNotExistException)
            {
                return NotFound();
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }

        [HttpDelete("deleteNote/{noteId:guid}/${userId:guid}")]
        public IActionResult DeleteNote(Guid noteId, Guid userId)
        {
            try
            {
                _noteService.DeleteNote(noteId, userId);
                return NoContent();
            }
            catch (ObjectDoesNotExistException)
            {
                return NotFound();
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }
        
        [HttpGet("searchTags/{groupId:guid}/{search}")]
        public IActionResult SearchTags(Guid groupId, string search)
        {
            try
            {
                return Ok(_noteService.SearchTags(groupId, search));
            }
            catch (ObjectDoesNotExistException)
            {
                return NotFound();
            }
        }
    }
}