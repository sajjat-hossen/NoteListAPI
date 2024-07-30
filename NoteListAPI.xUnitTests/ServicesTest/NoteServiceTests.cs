using AutoMapper;
using Moq;
using NoteListAPI.DomainLayer.Models;
using NoteListAPI.RepositoryLayer.IRepositories;
using NoteListAPI.ServiceLayer.Models;
using NoteListAPI.ServiceLayer.Services;
using System.Linq.Expressions;

namespace NoteListAPI.xUnitTests.ServicesTest
{
    public class NoteServiceTests
    {
        #region Fields

        private readonly NoteService _noteService;
        private readonly Mock<INoteRepository> _mockNoteRepository;
        private readonly Mock<IMapper> _mockMapper;

        #endregion

        #region Constructor

        public NoteServiceTests()
        {
            _mockNoteRepository = new Mock<INoteRepository>();
            _mockMapper = new Mock<IMapper>();
            _noteService = new NoteService(_mockNoteRepository.Object, _mockMapper.Object);
        }

        #endregion

        #region ConvertModelTests

        [Fact]
        public void MapNoteToNoteViewModel_ReturnsNoteViewModel()
        {
            // Arrange
            var note = new Note { Id = 1, Title = "Note 1", Description = "Description 1" };
            var noteViewModel = new NoteViewModel { Id = 1, Title = "Note 1", Description = "Description 1" };

            _mockMapper.Setup(m => m.Map<NoteViewModel>(note)).Returns(noteViewModel);

            // Act
            var result = _noteService.MapNoteToNoteViewModel(note);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(noteViewModel, result);
        }

        [Fact]
        public void MapNoteViewModelToNote_ReturnsNote()
        {
            // Arrange
            var noteViewModel = new NoteViewModel { Id = 1, Title = "Note 1", Description = "Description 1" };
            var note = new Note { Id = 1, Title = "Note 1", Description = "Description 1" };

            _mockMapper.Setup(m => m.Map<Note>(noteViewModel)).Returns(note);

            // Act
            var result = _noteService.MapNoteViewModelToNote(noteViewModel);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(note, result);
        }

        #endregion

        #region GetAllNotesAsyncTests

        [Fact]
        public async Task GetAllNoteAsync_ReturnsListOfNoteViewModel()
        {
            // Arrange
            var notes = new List<Note>
        {
            new Note { Id = 1, Title = "Note 1", Description = "Description 1" },
            new Note { Id = 2, Title = "Note 2", Description = "Description 2" }
        };

            var noteViewModels = new List<NoteViewModel>
        {
            new NoteViewModel { Id = 1, Title = "Note 1", Description = "Description 1" },
            new NoteViewModel { Id = 2, Title = "Note 2", Description = "Description 2" }
        };

            _mockNoteRepository.Setup(repo => repo.GetAllEntityFromDbAsync()).ReturnsAsync(notes);
            _mockMapper.Setup(m => m.Map<List<NoteViewModel>>(notes)).Returns(noteViewModels);

            // Act
            var result = await _noteService.GetAllNoteAsync();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal(noteViewModels, result);
        }

        #endregion

        #region CreateNoteAsyncTests

        [Fact]
        public async Task CreateNoteAsync_CreatesNoteSuccessfully()
        {
            // Arrange
            var createNoteModel = new CreateNote { Title = "New Note", Description = "New Content" };
            var note = new Note { Title = "New Note", Description = "New Content" };

            _mockMapper.Setup(m => m.Map<Note>(createNoteModel)).Returns(note);
            _mockNoteRepository.Setup(repo => repo.AddEntityAsync(note)).Returns(Task.CompletedTask);

            // Act
            await _noteService.CreateNoteAsync(createNoteModel);

            // Assert
            _mockNoteRepository.Verify(repo => repo.AddEntityAsync(note), Times.Once);
        }

        #endregion

        #region GetNoteByIdAsyncTests

        [Fact]
        public async Task GetNoteByIdAsync_ReturnsNote()
        {
            // Arrange
            var id = 1;
            var note = new Note { Id = id, Title = "Test Note", Description = "Content" };

            _mockNoteRepository.Setup(repo => repo.GetEntityByIdAsync(It.IsAny<Expression<Func<Note, bool>>>()))
            .ReturnsAsync(note);

            // Act
            var result = await _noteService.GetNoteByIdAsync(id);

            // Assert
            Assert.Equal(note, result);
        }

        #endregion

        #region RemoveNoteAsyncTests

        [Fact]
        public async Task RemoveNoteAsync_RemovesNoteSuccessfully()
        {
            // Arrange
            var note = new Note { Id = 1, Title = "Note 1", Description = "Content 1" };

            _mockNoteRepository.Setup(repo => repo.RemoveEntityAsync(note)).Returns(Task.CompletedTask);

            // Act
            await _noteService.RemoveNoteAsync(note);

            // Assert
            _mockNoteRepository.Verify(repo => repo.RemoveEntityAsync(note), Times.Once);
        }

        #endregion

        #region UpdateNoteAsync

        [Fact]
        public async Task UpdateNoteAsync_ReturnsTrue_WhenNoteExistsAndIsUpdated()
        {
            // Arrange
            var updateNoteModel = new UpdateNote { Id = 1, Title = "Updated Note", Description = "Updated Content" };
            var noteInDb = new Note { Id = 1, Title = "Old Note", Description = "Old Content" };

            _mockNoteRepository.Setup(repo => repo.GetEntityByIdAsync(It.IsAny<Expression<Func<Note, bool>>>()))
                .ReturnsAsync(noteInDb);
            _mockNoteRepository.Setup(repo => repo.UpdateEntityAsync(noteInDb))
                .Returns(Task.CompletedTask);

            _mockMapper.Setup(m => m.Map(updateNoteModel, noteInDb)).Callback(() =>
            {
                noteInDb.Title = updateNoteModel.Title;
                noteInDb.Description = updateNoteModel.Description;
            });

            // Act
            var result = await _noteService.UpdateNoteAsync(updateNoteModel);

            // Assert
            Assert.True(result);
            Assert.Equal(updateNoteModel.Title, noteInDb.Title);
            Assert.Equal(updateNoteModel.Description, noteInDb.Description);
        }

        [Fact]
        public async Task UpdateNoteAsync_ReturnsFalse_WhenNoteDoesNotExist()
        {
            // Arrange
            var updateNoteModel = new UpdateNote { Id = 1, Title = "Updated Note", Description = "Updated Content" };

            _mockNoteRepository.Setup(repo => repo.GetEntityByIdAsync(It.IsAny<Expression<Func<Note, bool>>>()))
                .ReturnsAsync((Note)null);

            // Act
            var result = await _noteService.UpdateNoteAsync(updateNoteModel);

            // Assert
            Assert.False(result);
        }

        #endregion
    }
}
