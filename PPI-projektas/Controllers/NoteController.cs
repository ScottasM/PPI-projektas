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

        [HttpGet("open/{id}")]
        public IActionResult OpenNote(Guid id)
        {
            return Ok(new NoteService().GetNote(id));
        }

        [HttpPost("updateNote")]
        public IActionResult UpdateNote([FromBody] Note noteData)
        {
            new NoteService().UpdateNote(noteData.Id, noteData.Name, noteData.Tags, noteData.Text);
            return Ok();
        }
    }
}