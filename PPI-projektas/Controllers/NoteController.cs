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

        [HttpGet("openNote/{noteId:guid}")]
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

        [HttpPost("createNote/{authorId}")]
        public IActionResult CreateNote(Guid authorId)
        {
            var noteId = new NoteService().CreateNote(authorId);
            return CreatedAtAction("CreateNote", noteId);
        }

        [HttpPost("updateNote/{noteId:guid}")]
        public IActionResult UpdateNote(Guid noteId, [FromBody] Note noteData)
        {
            try
            {
                new NoteService().UpdateNote(noteId, noteData.UserId, noteData.Name, noteData.Tags, noteData.Text);
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
                new NoteService().DeleteNote(noteId, userId);
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