using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NoteListAPI.ServiceLayer.IServices;
using NoteListAPI.ServiceLayer.Models;

namespace NoteListAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NoteController : ControllerBase
    {
        #region Fields

        private readonly INoteService _noteService;

        #endregion

        #region Constructor

        public NoteController(INoteService noteService)
        {
            _noteService = noteService;
        }

        #endregion

        #region GetAllNote

        [HttpGet("all")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Policy = "ViewNotePolicy")]

        public async Task<IActionResult> GetAllNote()
        {
            var notes = await _noteService.GetAllNoteAsync();
            
            if (notes != null)
            {
                return Ok(notes);
            }

            return NotFound("No note found");
        }

        #endregion

        #region CreateNote

        [HttpPost("add")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Policy = "CreateNotePolicy")]

        public async Task<IActionResult> CreateNote(CreateNote note)
        {
            if (note == null)
            {
                return BadRequest("Failed to create note");
            }

            await _noteService.CreateNoteAsync(note);

            return Ok("Note created successfully");
        }

        #endregion

        #region DeleteNote

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Policy = "DeleteNotePolicy")]

        public async Task<IActionResult> DeleteNote(int id)
        {
            if (id == 0)
            {
                return BadRequest("Note does not exists");
            }

            var note = await _noteService.GetNoteByIdAsync(id);

            if (note == null)
            {
                return NotFound("Note does not exists");
            }

            await _noteService.RemoveNoteAsync(note);

            return Ok("Note deleted successfully");
        }

        #endregion

        #region UpdateNote

        [HttpPut("Update")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Policy = "EditNotePolicy")]

        public async Task<IActionResult> UpdateNote(UpdateNote note)
        {
            if (note == null)
            {
                return BadRequest("Note does not exists");
            }

            var isSuccessful = await _noteService.UpdateNoteAsync(note);

            if (isSuccessful == false)
            {
                return NotFound("Note does not exists");
            }

            return Ok("Note updated successfully");
        }

        #endregion

        #region GetNoteById

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Policy = "ViewNotePolicy")]

        public async Task<IActionResult> GetNoteById(int id)
        {
            var note = _noteService.MapNoteToNoteViewModel(await _noteService.GetNoteByIdAsync(id));

            if (note == null)
            {
                return NotFound("Note does not exists.");
            }

            return Ok(note);
        }

        #endregion
    }
}
