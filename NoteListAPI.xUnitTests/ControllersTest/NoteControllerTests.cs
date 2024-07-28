using Microsoft.AspNetCore.Mvc;
using Moq;
using NoteListAPI.Controllers;
using NoteListAPI.ServiceLayer.IServices;
using NoteListAPI.ServiceLayer.Models;
using NoteListAPI.xUnitTests.ControllersTest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            Assert.Equal(200, result.StatusCode);
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
            Assert.Equal(404, notFoundResult.StatusCode);
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

                Assert.Equal(200, okResult.StatusCode);
                Assert.Equal("Note created successfully", okResult.Value);
            }
            else
            {
                var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);

                Assert.Equal(400, badRequestResult.StatusCode);
                Assert.Equal("Failed to create note", badRequestResult.Value);
            }

        }

        #endregion
    }
}
