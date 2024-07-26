using AutoMapper;
using NoteListAPI.DomainLayer.Models;
using NoteListAPI.ServiceLayer.Models;

namespace NoteListAPI.ServiceLayer.MappingProfiles
{
    public class Mappings : Profile
    {
        public Mappings()
        {
            CreateMap<NoteViewModel, Note>();
            CreateMap<Note, NoteViewModel>();
            CreateMap<CreateNote, Note>();
            CreateMap<UpdateNote, Note>();
            CreateMap<CreateTodoList, TodoList>();
            CreateMap<TodoList, TodoListViewModel>();
            CreateMap<TodoListViewModel, TodoList>();
            CreateMap<UpdateTodoList, TodoList>();
        }
    }
}
