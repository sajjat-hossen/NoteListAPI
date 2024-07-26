using NoteListAPI.DomainLayer.Models;
using NoteListAPI.ServiceLayer.Models;

namespace NoteListAPI.ServiceLayer.IServices
{
    public interface INoteService
    {
        Task<List<NoteViewModel>> GetAllNoteAsync();
        Task CreateNoteAsync(CreateNote note);
        Task<Note> GetNoteByIdAsync(int id);
        Task RemoveNoteAsync(Note note);
        Task<bool> UpdateNoteAsync(UpdateNote note);
        NoteViewModel MapNoteToNoteViewModel(Note note);
        Note MapNoteViewModelToNote(NoteViewModel model);
    }
}
