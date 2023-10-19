using System.Dynamic;
using Microsoft.AspNetCore.Mvc;
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

        [HttpGet("open/{id:guid}")]
        public IActionResult OpenNote(Guid id)
        {
            return Ok(new NoteService().GetNote(id));
        }

        [HttpPost("updateNote/{noteId:guid}")]
        public IActionResult UpdateNote(Guid noteId, [FromBody] Note noteData)
        {
            new NoteService().UpdateNote(noteId, noteData.AuthorGuid, noteData.Name, noteData.Tags, noteData.Text);
            return Ok();
        }
    }
}