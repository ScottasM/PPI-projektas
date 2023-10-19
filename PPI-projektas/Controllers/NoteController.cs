using System.Dynamic;
using Microsoft.AspNetCore.Mvc;
using PPI_projektas.Exceptions;
using PPI_projektas.objects;
using PPI_projektas.Services;

namespace PPI_projektas.Controllers

{
    [ApiController]
    [Route("api/[controller]")]
    public class NoteController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new NoteService().GetNotes());
        }

        [HttpGet("openNote/{noteId}")]
        public IActionResult OpenNote(Guid noteId)
        {
            try
            {
                return Ok(new NoteService().GetNote(noteId));
            }
            catch (ObjectDoesNotExistException)
            {
                return NotFound();
            }
        }

        [HttpPost("createNote")]
        public IActionResult CreateNote([FromBody] Guid authorId)
        {
            return CreatedAtAction("CreateNote",new NoteService().CreateNote(authorId));
        }

        [HttpPost("updateNote/{noteId}")]
        public IActionResult UpdateNote(Guid noteId, [FromBody] Note noteData)
        {
            try
            {
                new NoteService().UpdateNote(noteId, noteData.AuthorId, noteData.Name, noteData.Tags, noteData.Text);
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
        public IActionResult DeleteNote(Guid noteId, [FromBody] Guid authorId)
        {
            try
            {
                new NoteService().DeleteNote(noteId, authorId);
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