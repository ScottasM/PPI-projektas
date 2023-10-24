using Microsoft.AspNetCore.Mvc;
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

        [HttpGet("open/{id:guid}")]
        public IActionResult OpenNote(Guid id)
        {
            return Ok(_noteService.GetNote(id));
        }

        [HttpPost("updateNote/{noteId:guid}")]
        public IActionResult UpdateNote(Guid noteId, [FromBody] Note noteData)
        {
            _noteService.UpdateNote(noteId, noteData.AuthorGuid, noteData.Name, noteData.Tags, noteData.Text);
            return Ok();
        }
    }
}