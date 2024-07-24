using AutoMapper;
using NoteListAPI.DomainLayer.Models;
using NoteListAPI.RepositoryLayer.IRepositories;
using NoteListAPI.ServiceLayer.IServices;
using NoteListAPI.ServiceLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoteListAPI.ServiceLayer.Services
{
    public class TodoListService : ITodoListService
    {
        #region Fields

        private readonly ITodoListRepository _todoListRepository;
        private readonly IMapper _mapper;

        #endregion

        #region Constructor

        public TodoListService(ITodoListRepository todoListRepository, IMapper mapper)
        {
            _todoListRepository = todoListRepository;
            _mapper = mapper;
        }

        #endregion

        //#region MapTodoListToTodoListViewModel

        //public TodoListViewModel MapTodoListToTodoListViewModel(TodoList todoList)
        //{
        //    var todoListViewModel = _mapper.Map<TodoListViewModel>(todoList);

        //    return todoListViewModel;
        //}

        //#endregion

        //#region MapTodoListViewModelToTodoList

        //public TodoList MapTodoListViewModelToTodoList(TodoListViewModel model)
        //{
        //    var todoList = _mapper.Map<TodoList>(model);

        //    return todoList;
        //}

        //#endregion

        //#region GetAllTodoListAsync

        //public async Task<List<TodoListViewModel>> GetAllTodoListAsync()
        //{
        //    var todoList = await _todoListRepository.GetAllEntityFromDbAsync();
        //    var todoListViewModel = _mapper.Map<List<TodoListViewModel>>(todoList);

        //    return todoListViewModel;
        //}

        //#endregion

        #region CreateTodoListAsync

        public async Task CreateTodoListAsync(CreateTodoList model)
        {
            var todoList = _mapper.Map<TodoList>(model);
            await _todoListRepository.AddEntityAsync(todoList);
        }

        #endregion

        //#region GetTodoListByIdAsync

        //public async Task<TodoList> GetTodoListByIdAsync(int id)
        //{
        //    var todoList = await _todoListRepository.GetEntityByIdAsync(i => i.Id == id);

        //    return todoList;
        //}

        //#endregion

        //#region RemoveTodoListAsync

        //public async Task RemoveTodoListAsync(TodoList todoList)
        //{
        //    await _todoListRepository.RemoveEntityAsync(todoList);
        //}

        //#endregion

        //#region UpdateTodoListAsync

        //public async Task UpdateTodoListAsync(TodoListViewModel model)
        //{
        //    var note = MapTodoListViewModelToTodoList(model);
        //    await _todoListRepository.UpdateEntityAsync(note);
        //}

        //#endregion
    }
}
