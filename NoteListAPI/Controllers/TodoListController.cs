using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NoteListAPI.ServiceLayer.IServices;
using NoteListAPI.ServiceLayer.Models;

namespace NoteListAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class TodoListController : ControllerBase
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

        #region GetAllTodoList

        [HttpGet("all")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Policy = "ViewTodoListPolicy")]

        public async Task<IActionResult> GetAllTodoList()
        {
            var todoLists = await _todoListService.GetAllTodoListAsync();

            if (todoLists != null)
            {
                return Ok(todoLists);
            }

            return NotFound("No todo found");
        }

        #endregion

        #region CreateTodoList

        [HttpPost("add")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Policy = "CreateTodoListPolicy")]

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

        #region DeleteTodoList

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Policy = "DeleteTodoListPolicy")]

        public async Task<IActionResult> DeleteTodoList(int id)
        {
            if (id == 0)
            {
                return BadRequest("Todo does not exists");
            }

            var todoList = await _todoListService.GetTodoListByIdAsync(id);

            if (todoList == null)
            {
                return NotFound("Todo does not exists");
            }

            await _todoListService.RemoveTodoListAsync(todoList);

            return Ok("Todo deleted successfully");
        }

        #endregion

        #region UpdateNote

        [HttpPut("Update")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Policy = "EditTodoListPolicy")]

        public async Task<IActionResult> UpdateTodoList(UpdateTodoList todoList)
        {
            if (todoList == null)
            {
                return BadRequest("Todo does not exists");
            }

            var isSuccessful = await _todoListService.UpdateTodoListAsync(todoList);

            if (isSuccessful == false)
            {
                return NotFound("Todo does not exists");
            }

            return Ok("Todo updated successfully");
        }

        #endregion

        #region GetTodoListById

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Policy = "ViewTodoListPolicy")]

        public async Task<IActionResult> GetTodoListById(int id)
        {
            var todoList = _todoListService.MapTodoListToTodoListViewModel(await _todoListService.GetTodoListByIdAsync(id));

            if (todoList == null)
            {
                return NotFound("Todo does not exists.");
            }

            return Ok(todoList);
        }

        #endregion
    }
}
