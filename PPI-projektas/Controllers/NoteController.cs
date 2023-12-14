using System.Security.AccessControl;
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
        
        [HttpGet("{groupId:guid}")]
        public IActionResult Get(Guid groupId)
        {
            return Ok(_noteService.GetNotes(groupId));
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

        [HttpPost("createNote/{groupId:guid}/{authorId:guid}")]
        public IActionResult CreateNote(Guid groupId, Guid authorId)
        {
            try
            {
                var noteId = _noteService.CreateNote(groupId, authorId);
                return CreatedAtAction("CreateNote", noteId);
            }
            catch (ObjectDoesNotExistException)
            {
                return NotFound();
            }
        }

        [HttpGet("getPrivileges/{noteId:guid}")]
        public IActionResult GetPrivileges(Guid noteId)
        {
            try
            {
                return Ok(_noteService.GetPrivileges(noteId));
            }
            catch (ObjectDoesNotExistException)
            {
                return NotFound();
            }
        }

        [HttpPost("updatePrivileges/{noteId:guid}")]
        public IActionResult UpdatePrivileges(Guid noteId, [FromBody] UpdatePrivilegesData updatePrivileges)
        {
            try
            {
                return Ok(_noteService.UpdatePrivileges(noteId, updatePrivileges.UserId,
                    updatePrivileges.UpdatePrivileges));
            }
            catch (PrivilegeNotHeldException)
            {
                return Unauthorized();
            }
            catch (ArgumentOutOfRangeException)
            {
                return BadRequest();
            }
            catch (ObjectDoesNotExistException)
            {
                return NotFound();
            }
        }
        
        [HttpPost("updateNote/{noteId:guid}")]
        public IActionResult UpdateNote(Guid noteId, [FromBody] Note noteData)
        {
            try
            {

                _noteService.UpdateNote(noteId, noteData.UserId, noteData.Name, noteData.Tags, noteData.Text);

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