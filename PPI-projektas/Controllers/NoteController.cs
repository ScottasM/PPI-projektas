using Microsoft.AspNetCore.Mvc;
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
        public IActionResult Get([FromBody] NoteSearchData data)
        {
            return Ok(_noteService.GetNotes(data.UserId, data.GroupId, data.SearchType, data.Tags, data.NameFilter));
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

        [HttpPost("createNote/")]
        public IActionResult CreateNote([FromBody] NoteCreationData data)
        {
            try
            {
                var noteId = _noteService.CreateNote(data.GroupId, data.AuthorId);
                return CreatedAtAction("CreateNote", noteId);
            }
            catch (ObjectDoesNotExistException)
            {
                return NotFound();
            }
        }

        [HttpPost("updateNote/{noteId:guid}")]
        public IActionResult UpdateNote(Guid noteId, [FromBody] NoteUpdateData data)
        {
            try
            {
                _noteService.UpdateNote(noteId, data.UserId, data.Name, data.Tags, data.Text);
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