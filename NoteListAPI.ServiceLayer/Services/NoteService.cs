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
    public class NoteService : INoteService
    {
        #region Fields

        private readonly INoteRepository _noteRepository;
        private readonly IMapper _mapper;

        #endregion

        #region Constructor

        public NoteService(INoteRepository noteRepository, IMapper mapper)
        {
            _noteRepository = noteRepository;
            _mapper = mapper;
        }

        #endregion

        #region MapNoteToNoteViewModel

        public NoteViewModel MapNoteToNoteViewModel(Note note)
        {
            var noteViewModel = _mapper.Map<NoteViewModel>(note);

            return noteViewModel;
        }

        #endregion

        #region MapNoteViewModelToNote

        public Note MapNoteViewModelToNote(NoteViewModel model)
        {
            var note = _mapper.Map<Note>(model);

            return note;
        }

        #endregion

        #region GetAllNoteAsync

        public async Task<List<NoteViewModel>> GetAllNoteAsync()
        {
            var notes = await _noteRepository.GetAllEntityFromDbAsync();
            var notesViewModel = _mapper.Map<List<NoteViewModel>>(notes);

            return notesViewModel;
        }

        #endregion

        #region CreateNoteAsync

        public async Task CreateNoteAsync(NoteViewModel model)
        {
            var note = MapNoteViewModelToNote(model);
            await _noteRepository.AddEntityAsync(note);
        }

        #endregion

        #region GetNoteByIdAsync

        public async Task<Note> GetNoteByIdAsync(int id)
        {
            var note = await _noteRepository.GetEntityByIdAsync(i => i.Id == id);

            return note;
        }

        #endregion

        #region RemoveNoteAsync

        public async Task RemoveNoteAsync(Note note)
        {
            await _noteRepository.RemoveEntityAsync(note);
        }

        #endregion

        #region UpdateNoteAsync

        public async Task UpdateNoteAsync(NoteViewModel model)
        {
            var note = MapNoteViewModelToNote(model);
            await _noteRepository.UpdateEntityAsync(note);
        }

        #endregion
    }
}
