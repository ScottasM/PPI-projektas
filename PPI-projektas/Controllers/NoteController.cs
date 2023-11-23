using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using PPI_projektas.Exceptions;
using PPI_projektas.objects;
using PPI_projektas.Services.Interfaces;
using PPI_projektas.Services.Request;

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
        
        [HttpGet]
        [Route("/search")]
        public IActionResult Get([FromQuery] NoteSearchData search)
        {
            return Ok(_noteService.GetNotes(search.UserId, search.SearchType, search.TagFilter, search.NameFilter, search.GroupId));
        }

        [HttpGet("openNote/{noteId:guid}")]
        public IActionResult OpenNote(Guid noteId, [FromBody] Guid userId)
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
        public IActionResult UpdateNote(Guid noteId, [FromBody] Note data)
        {
            try
            {
                _noteService.UpdateNote(noteId, data.AuthorId, data.Name, data.Tags.Select(tag => tag.Text), data.Text);
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

        [HttpDelete("deleteNote/{noteId}")]
        public IActionResult DeleteNote(Guid noteId, [FromBody] Guid userId)
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
    }
}