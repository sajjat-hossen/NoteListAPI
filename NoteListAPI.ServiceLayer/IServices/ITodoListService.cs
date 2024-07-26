using NoteListAPI.DomainLayer.Models;
using NoteListAPI.ServiceLayer.Models;

namespace NoteListAPI.ServiceLayer.IServices
{
    public interface ITodoListService
    {
        Task<List<TodoListViewModel>> GetAllTodoListAsync();
        Task CreateTodoListAsync(CreateTodoList model);
        Task<TodoList> GetTodoListByIdAsync(int id);
        Task RemoveTodoListAsync(TodoList todoList);
        Task<bool> UpdateTodoListAsync(UpdateTodoList model);
        TodoListViewModel MapTodoListToTodoListViewModel(TodoList todoList);
        TodoList MapTodoListViewModelToTodoList(TodoListViewModel model);
    }
}
