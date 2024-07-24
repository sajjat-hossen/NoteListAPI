using Microsoft.AspNetCore.Mvc;
using NoteListAPI.ServiceLayer.IServices;
using NoteListAPI.ServiceLayer.Models;

namespace NoteListAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoListController : Controller
    {
        #region Fields

        private readonly ITodoListService _todoListService;

        #endregion

        #region Constructor

        public TodoListController(ITodoListService todoListService)
        {
            _todoListService = todoListService;
        }

        #endregion

        //#region GetAllNote

        //[HttpGet("all")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]

        //public async Task<IActionResult> GetAllNote()
        //{
        //    var notes = await _noteService.GetAllNoteAsync();

        //    if (notes != null)
        //    {
        //        return Ok(notes);
        //    }

        //    return NotFound("No note found");
        //}

        //#endregion

        #region CreateTodoList

        [HttpPost("add")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<IActionResult> CreateTodoList(CreateTodoList todoList)
        {
            if (todoList == null)
            {
                return BadRequest("Failed to create todo list");
            }

            await _todoListService.CreateTodoListAsync(todoList);

            return Ok("Todo list created successfully");
        }

        #endregion

        //#region RemoveNote

        //[HttpDelete("{id:int}", Name = "Delete")]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(StatusCodes.Status200OK)]

        //public async Task<IActionResult> DeleteNote(int id)
        //{
        //    if (id == 0)
        //    {
        //        return BadRequest("Note does not exists");
        //    }

        //    var note = await _noteService.GetNoteByIdAsync(id);

        //    if (note == null)
        //    {
        //        return NotFound("Note does not exists");
        //    }

        //    await _noteService.RemoveNoteAsync(note);

        //    return Ok("Note deleted successfully");
        //}

        //#endregion

        //#region UpdateNote

        //[HttpPut("Update")]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]

        //public async Task<IActionResult> UpdateNote(UpdateNote note)
        //{
        //    if (note == null)
        //    {
        //        return BadRequest("Note does not exists");
        //    }

        //    var isSuccessful = await _noteService.UpdateNoteAsync(note);

        //    if (isSuccessful == false)
        //    {
        //        return NotFound("Note does not exists");
        //    }

        //    return Ok("Note updated successfully");
        //}

        //#endregion

        //#region GetNoteById

        //[HttpGet("{id:int}")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]

        //public async Task<IActionResult> GetNoteById(int id)
        //{
        //    var note = _noteService.MapNoteToNoteViewModel(await _noteService.GetNoteByIdAsync(id));

        //    if (note == null)
        //    {
        //        return NotFound("Note does not exists.");
        //    }

        //    return Ok(note);
        //}

        //#endregion
    }
}
