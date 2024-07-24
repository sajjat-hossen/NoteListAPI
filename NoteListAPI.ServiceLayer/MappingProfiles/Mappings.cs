using AutoMapper;
using NoteListAPI.DomainLayer.Models;
using NoteListAPI.ServiceLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        }
    }
}
