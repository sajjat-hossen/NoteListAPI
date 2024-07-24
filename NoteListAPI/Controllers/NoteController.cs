﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NoteListAPI.ServiceLayer.IServices;
using NoteListAPI.ServiceLayer.Models;

namespace NoteListAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NoteController : Controller
    {
        #region Fields

        private readonly INoteService _noteService;

        #endregion

        #region Constructor

        public NoteController(INoteService noteService)
        {
            _noteService = noteService;
        }

        #endregion

        #region GetAllNote

        [HttpGet("all")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<IActionResult> GetAllNote()
        {
            var notes = await _noteService.GetAllNoteAsync();
            
            if (notes != null)
            {
                return Ok(notes);
            }

            return NotFound("No note found");
        }

        #endregion

        #region CreateNote

        [HttpPost("add")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<IActionResult> CreateNote(CreateNote note)
        {
            if (note == null)
            {
                return BadRequest("Failed to create note");
            }

            await _noteService.CreateNoteAsync(note);

            return Ok("Note created successfully");
        }

        #endregion

        #region RemoveNote

        [HttpDelete("{id:int}", Name = "RemoveNote")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]

        public async Task<IActionResult> RemoveNote(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }

            var note = _noteService.MapNoteToNoteViewModel(await _noteService.GetNoteByIdAsync(id));

            if (note == null)
            {
                return NotFound();
            }

            return NoContent();
        }

        #endregion

        #region UpdateNote

        [HttpPut("Update")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<IActionResult> UpdateNote(UpdateNote note)
        {
            if (note == null)
            {
                return BadRequest("Note does not exists");
            }

            var isSuccessful = await _noteService.UpdateNoteAsync(note);

            if (isSuccessful == false)
            {
                return NotFound("Note does not exists");
            }

            return Ok("Note updated successfully");
        }

        #endregion

        #region GetNoteById

        [HttpGet("{id:int}", Name = "GetNoteById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<IActionResult> GetNoteById(int id)
        {
            var note = _noteService.MapNoteToNoteViewModel(await _noteService.GetNoteByIdAsync(id));

            if (note == null)
            {
                return NotFound("Note does not exists");
            }

            return Ok(note);
        }

        #endregion
    }
}
