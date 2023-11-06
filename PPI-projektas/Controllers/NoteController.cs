using Microsoft.AspNetCore.Mvc;
using PPI_projektas.Exceptions;
using PPI_projektas.objects;
using PPI_projektas.Services.Interfaces;

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
        public IActionResult Get()
        {
            return Ok(_noteService.GetNotes());
        }

        [HttpGet("openNote/{noteId:guid}")]
        public IActionResult OpenNote(Guid noteId)
        {
            try
            {
                return Ok(_noteService.GetNote(noteId));
            }
            catch (ObjectDoesNotExistException)
            {
                return NotFound();
            }
        }

        [HttpPost("createNote/{authorId}")]
        public IActionResult CreateNote(Guid authorId)
        {
            var noteId = _noteService.CreateNote(authorId);
            return CreatedAtAction("CreateNote", noteId);
        }

        [HttpPost("updateNote/{noteId:guid}")]
        public IActionResult UpdateNote(Guid noteId, [FromBody] Note noteData)
        {
            try
            {
                _noteService.UpdateNote(noteId, noteData.AuthorId, noteData.Name, noteData.Tags, noteData.Text);
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

        [HttpDelete("deleteNote/{noteId}/{userId}")]
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
    }
}