using NoteListAPI.DomainLayer.Models;
using NoteListAPI.ServiceLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoteListAPI.ServiceLayer.IServices
{
    public interface INoteService
    {
        Task<List<NoteViewModel>> GetAllNoteAsync();
        Task CreateNoteAsync(NoteViewModel note);
        Task<Note> GetNoteByIdAsync(int id);
        Task RemoveNoteAsync(Note note);
        Task UpdateNoteAsync(NoteViewModel note);
        NoteViewModel MapNoteToNoteViewModel(Note note);
        Note MapNoteViewModelToNote(NoteViewModel model);
    }
}
