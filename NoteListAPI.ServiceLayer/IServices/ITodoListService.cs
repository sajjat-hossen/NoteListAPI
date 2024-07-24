using NoteListAPI.DomainLayer.Models;
using NoteListAPI.ServiceLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoteListAPI.ServiceLayer.IServices
{
    public interface ITodoListService
    {
        Task<List<TodoListViewModel>> GetAllTodoListAsync();
        Task CreateTodoListAsync(CreateTodoList model);
        Task<TodoList> GetTodoListByIdAsync(int id);
        Task RemoveTodoListAsync(TodoList todoList);
        //Task UpdateTodoListAsync(TodoListViewModel model);
        TodoListViewModel MapTodoListToTodoListViewModel(TodoList todoList);
        TodoList MapTodoListViewModelToTodoList(TodoListViewModel model);
    }
}
