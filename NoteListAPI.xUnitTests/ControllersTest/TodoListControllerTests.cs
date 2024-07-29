using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NoteListAPI.Controllers;
using NoteListAPI.DomainLayer.Models;
using NoteListAPI.ServiceLayer.IServices;
using NoteListAPI.ServiceLayer.Models;

namespace NoteListAPI.xUnitTests.ControllersTest
{
    public class TodoListControllerTests
    {
        #region Fields

        private readonly Mock<ITodoListService> _mockTodoListService;
        private readonly TodoListController _todoListController;

        #endregion

        #region Constructor

        public TodoListControllerTests()
        {
            _mockTodoListService = new Mock<ITodoListService>();
            _todoListController = new TodoListController(_mockTodoListService.Object);
        }

        #endregion

        #region GetAllTodoListsTests

        [Fact]
        public async Task GetAllTodoLists_ReturnsOkResult_WhenTodoListsExist()
        {
            // Arrange
            var todoLists = new List<TodoListViewModel>
            {
                new TodoListViewModel { Id = 1, Title = "Title 1", Description = "Description 1" },
                new TodoListViewModel { Id = 2, Title = "Title 2", Description = "Description 2" }
            };
            _mockTodoListService.Setup(service => service.GetAllTodoListAsync())
                .ReturnsAsync(todoLists);

            // Act
            var result = await _todoListController.GetAllTodoList() as OkObjectResult;

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnNotes = Assert.IsType<List<TodoListViewModel>>(okResult.Value);

            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
            Assert.Equal(2, returnNotes.Count);
            Assert.Equal(todoLists, result.Value);
        }

        [Fact]
        public async Task GetAllTodoLists_ReturnsNotFound_WhenNoTodoListsExist()
        {
            // Arrange
            _mockTodoListService.Setup(service => service.GetAllTodoListAsync())
                .ReturnsAsync((List<TodoListViewModel>)null);

            // Act
            var result = await _todoListController.GetAllTodoList();

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
            Assert.Equal("No todo found", notFoundResult.Value);
        }

        #endregion

        #region CreateTodoListTests

        public static IEnumerable<object[]> CreateTodoListData()
        {
            return new List<object[]>
            {
                new object[] { new CreateTodoList { Title = "Title 1", Description = "Description 1" } },
                new object[] { new CreateTodoList { Title = "Title 2", Description = "Description 2" } },
                new object[] { null }
            };
        }

        [Theory]
        [MemberData(nameof(CreateTodoListData))]

        public async Task CreateTodoList_ReturnsExpectedResult(CreateTodoList todo)
        {
            // Arrange
            _mockTodoListService.Setup(service => service.CreateTodoListAsync(todo))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _todoListController.CreateTodoList(todo);

            // Assert
            if (todo != null)
            {
                var okResult = Assert.IsType<OkObjectResult>(result);

                Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
                Assert.Equal("Todo list created successfully", okResult.Value);
            }
            else
            {
                var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);

                Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
                Assert.Equal("Failed to create todo list", badRequestResult.Value);
            }

        }

        #endregion

        #region DeleteTodoListTests
        public static IEnumerable<object[]> DeleteTodoListData()
        {
            return new List<object[]>
            {
                new object[] { 1, true },
                new object[] { 0, false },
                new object[] { 2, false }
            };

        }

        [Theory]
        [MemberData(nameof(DeleteTodoListData))]
        public async Task DeleteTodoList_ReturnsExpectedResult(int id, bool todoListExists)
        {
            // Arrange
            if (id == 0)
            {

            }
            else if (todoListExists)
            {
                var todo = new TodoList
                {
                    Id = id,
                    Title = "Existing Note",
                    Description = "Existing Description"
                };

                _mockTodoListService.Setup(service => service.GetTodoListByIdAsync(id)).ReturnsAsync(todo);
                _mockTodoListService.Setup(service => service.RemoveTodoListAsync(todo)).Returns(Task.CompletedTask);
            }
            else
            {
                _mockTodoListService.Setup(service => service.GetTodoListByIdAsync(id)).ReturnsAsync((TodoList)null);
            }

            // Act
            var result = await _todoListController.DeleteTodoList(id);

            // Assert
            if (id == 0)
            {
                var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
                Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
                Assert.Equal("Todo does not exists", badRequestResult.Value);
                _mockTodoListService.Verify(service => service.RemoveTodoListAsync(It.IsAny<TodoList>()), Times.Never);
            }
            else if (todoListExists)
            {
                var okResult = Assert.IsType<OkObjectResult>(result);
                Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
                Assert.Equal("Todo deleted successfully", okResult.Value);
                _mockTodoListService.Verify(service => service.RemoveTodoListAsync(It.IsAny<TodoList>()), Times.Once);
            }
            else
            {
                var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
                Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
                Assert.Equal("Todo does not exists", notFoundResult.Value);
                _mockTodoListService.Verify(service => service.RemoveTodoListAsync(It.IsAny<TodoList>()), Times.Never);
            }
        }

        #endregion

        #region UpdateTodoListTests

        [Fact]
        public async Task UpdateTodoList_ReturnsOkResult_WhenTodoListIsUpdated()
        {
            // Arrange
            var updateTodoList = new UpdateTodoList { Id = 1, Title = "Updated title", Description = "Updated Description" };
            _mockTodoListService.Setup(service => service.UpdateTodoListAsync(updateTodoList))
                .ReturnsAsync(true);

            // Act
            var result = await _todoListController.UpdateTodoList(updateTodoList);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);

            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.Equal("Todo updated successfully", okResult.Value);
        }

        [Fact]
        public async Task UpdateTodoList_ReturnsNotFound_WhenTodoListDoesNotExist()
        {
            // Arrange
            var updateTodoList = new UpdateTodoList { Id = 1, Title = "Updated title", Description = "Updated Description" };
            _mockTodoListService.Setup(service => service.UpdateTodoListAsync(updateTodoList))
                .ReturnsAsync(false);

            // Act
            var result = await _todoListController.UpdateTodoList(updateTodoList);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);

            Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
            Assert.Equal("Todo does not exists", notFoundResult.Value);
        }

        #endregion

        #region GetTodoListByIdTests

        [Fact]
        public async Task GetTodoListById_ReturnsOkResult_WhenTodoListExists()
        {
            // Arrange
            var todoListViewModel = new TodoListViewModel { Id = 1, Title = "Existing title", Description = "Existing Description" };
            var todo = new TodoList { Id = 1, Title = "Existing title", Description = "Existing Description" };

            _mockTodoListService.Setup(service => service.GetTodoListByIdAsync(1)).ReturnsAsync(todo);
            _mockTodoListService.Setup(service => service.MapTodoListToTodoListViewModel(todo))
                .Returns(todoListViewModel);

            // Act
            var result = await _todoListController.GetTodoListById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);

            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
        }

        [Fact]
        public async Task GetTodoListById_ReturnsNotFound_WhenTodoListDoesNotExist()
        {
            // Arrange
            _mockTodoListService.Setup(service => service.GetTodoListByIdAsync(1))
                .ReturnsAsync((TodoList)null);

            // Act
            var result = await _todoListController.GetTodoListById(1);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);

            Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
            Assert.Equal("Todo does not exists.", notFoundResult.Value);
        }

        #endregion
    }
}
