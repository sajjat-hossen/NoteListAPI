using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NoteListAPI.Controllers;
using NoteListAPI.DomainLayer.Models;
using NoteListAPI.ServiceLayer.IServices;
using NoteListAPI.ServiceLayer.Models;

namespace NoteListAPI.xUnitTests.Controllers
{
    public class NoteControllerTests
    {
        #region Fields

        private readonly Mock<INoteService> _mockNoteService;
        private readonly NoteController _noteController;

        #endregion

        #region Constructor

        public NoteControllerTests()
        {
            _mockNoteService = new Mock<INoteService>();
            _noteController = new NoteController(_mockNoteService.Object);
        }

        #endregion

        #region GetAllNoteTests

        [Fact]
        public async Task GetAllNote_ReturnsOkResult_WhenNotesExist()
        {
            // Arrange
            var notes = new List<NoteViewModel>
            {
                new NoteViewModel { Id = 1, Title = "Note 1", Description = "Content 1" },
                new NoteViewModel { Id = 2, Title = "Note 2", Description = "Content 2" }
            };
            _mockNoteService.Setup(service => service.GetAllNoteAsync())
                .ReturnsAsync(notes);

            // Act
            var result = await _noteController.GetAllNote() as OkObjectResult;

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnNotes = Assert.IsType<List<NoteViewModel>>(okResult.Value);

            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
            Assert.Equal(2, returnNotes.Count);
            Assert.Equal(notes, result.Value);
        }

        [Fact]
        public async Task GetAllNote_ReturnsNotFound_WhenNoNotesExist()
        {
            // Arrange
            _mockNoteService.Setup(service => service.GetAllNoteAsync())
                .ReturnsAsync((List<NoteViewModel>)null);

            // Act
            var result = await _noteController.GetAllNote();

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
            Assert.Equal("No note found", notFoundResult.Value);
        }

        #endregion

        #region CreateNoteTests

        public static IEnumerable<object[]> CreateNoteData()
        {
            return new List<object[]>
            {
                new object[] { new CreateNote { Title = "Title 1", Description = "Description 1" } },
                new object[] { new CreateNote { Title = "Title 2", Description = "Description 2" } },
                new object[] { null }
            };
        }

        [Theory]
        [MemberData(nameof(CreateNoteData))]

        public async Task CreateNote_ReturnsExpectedResult(CreateNote note)
        {
            // Arrange
            _mockNoteService.Setup(service => service.CreateNoteAsync(note))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _noteController.CreateNote(note);

            // Assert
            if (note != null)
            {
                var okResult = Assert.IsType<OkObjectResult>(result);

                Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
                Assert.Equal("Note created successfully", okResult.Value);
            }
            else
            {
                var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);

                Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
                Assert.Equal("Failed to create note", badRequestResult.Value);
            }

        }

        #endregion

        #region DeleteNoteTests
        public static IEnumerable<object[]> DeleteNoteData()
        {
            return new List<object[]>
            {
                new object[] { 1, true },
                new object[] { 0, false },
                //new object[] { 2, false }
            };

        }

        [Theory]
        [MemberData(nameof(DeleteNoteData))]
        public async Task DeleteNote_ReturnsExpectedResult(int id, bool noteExists)
        {
            // Arrange
            if (id == 0)
            {

            }
            else if (noteExists)
            {
                var note = new Note
                {
                    Id = id,
                    Title = "Existing Note",
                    Description = "Existing Description"
                };

                _mockNoteService.Setup(service => service.GetNoteByIdAsync(id)).ReturnsAsync(note);
                _mockNoteService.Setup(service => service.RemoveNoteAsync(note)).Returns(Task.CompletedTask);
            }
            else
            {
                _mockNoteService.Setup(service => service.GetNoteByIdAsync(id)).ReturnsAsync((Note)null);
            }

            // Act
            var result =  await _noteController.DeleteNote(id);

            // Assert
            if (id == 0)
            {
                var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
                Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
                Assert.Equal("Note does not exists", badRequestResult.Value);
                _mockNoteService.Verify(service => service.RemoveNoteAsync(It.IsAny<Note>()), Times.Never);
            }
            else if (noteExists)
            {
                var okResult = Assert.IsType<OkObjectResult>(result);
                Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
                Assert.Equal("Note deleted successfully", okResult.Value);
                _mockNoteService.Verify(service => service.RemoveNoteAsync(It.IsAny<Note>()), Times.Once);
            }
            else
            {
                var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
                Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
                Assert.Equal("Note does not exists", notFoundResult.Value);
                _mockNoteService.Verify(service => service.RemoveNoteAsync(It.IsAny<Note>()), Times.Never);
            }
        }

        #endregion

        #region UpdateNoteTests

        [Fact]
        public async Task UpdateNote_ReturnsOkResult_WhenNoteIsUpdated()
        {
            // Arrange
            var updateNote = new UpdateNote { Id = 1, Title = "Updated Note", Description = "Updated Description" };
            _mockNoteService.Setup(service => service.UpdateNoteAsync(updateNote))
                .ReturnsAsync(true);

            // Act
            var result = await _noteController.UpdateNote(updateNote);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);

            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.Equal("Note updated successfully", okResult.Value);
        }

        [Fact]
        public async Task UpdateNote_ReturnsNotFound_WhenNoteDoesNotExist()
        {
            // Arrange
            var updateNote = new UpdateNote { Id = 1, Title = "Updated Note", Description = "Updated Description" };
            _mockNoteService.Setup(service => service.UpdateNoteAsync(updateNote))
                .ReturnsAsync(false);

            // Act
            var result = await _noteController.UpdateNote(updateNote);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);

            Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
            Assert.Equal("Note does not exists", notFoundResult.Value);
        }

        #endregion

        #region GetNoteByIdTests

        public async Task GetNoteById_ReturnsOkResult_WhenNoteExists()
        {
            // Arrange
            var noteViewModel = new NoteViewModel { Id = 1, Title = "Existing Note", Description = "Existing Description" };
            var note = new Note { Id = 1, Title = "Existing Note", Description = "Existing Description" };

            _mockNoteService.Setup(service => service.GetNoteByIdAsync(1)).ReturnsAsync(note);
            _mockNoteService.Setup(service => service.MapNoteToNoteViewModel(note))
                .Returns(noteViewModel);

            // Act
            var result = await _noteController.GetNoteById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);

            Assert.Equal(note, okResult.Value);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
        }

        [Fact]
        public async Task GetNoteById_ReturnsNotFound_WhenNoteDoesNotExist()
        {
            // Arrange
            _mockNoteService.Setup(service => service.GetNoteByIdAsync(1))
                .ReturnsAsync((Note)null);

            // Act
            var result = await _noteController.GetNoteById(1);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);

            Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
            Assert.Equal("Note does not exists.", notFoundResult.Value);
        }

        #endregion
    }
}
