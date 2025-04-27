using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ToDoApp.Controllers;
using ToDoApp.Dtos;
using ToDoApp.Services;
using Xunit;

namespace ToDoApp.Tests.Controllers;

public class ToDoControllerTests
{
    private readonly Mock<IToDoService> _mockService;
    private readonly ToDoController _controller;

    public ToDoControllerTests()
    {
        _mockService = new Mock<IToDoService>();
        _controller = new ToDoController(_mockService.Object);
    }


    [Fact]
    public async Task Create_Should_Return_Created_At_Action()
    {
        var dto = new ToDoCreateDto
        {
            Title = "Test Task",
            Description = "Description of the task",
            ExpiryDate = DateTime.UtcNow.AddDays(1)
        };

        var createdTodo = new ToDoDto
        {
            Id = 1,
            Title = "Test Task",
            Description = "Description of the task",
            ExpiryDate = DateTime.UtcNow.AddDays(1)
        };

        _mockService.Setup(service => service.CreateAsync(It.IsAny<ToDoCreateDto>()))
                    .ReturnsAsync(createdTodo);

        var result = await _controller.Create(dto);

        var createdResult = result as CreatedAtActionResult;
        createdResult.Should().NotBeNull();
        createdResult?.StatusCode.Should().Be(201);
        createdResult?.Value.Should().BeEquivalentTo(createdTodo);
    }

    [Fact]
    public async Task GetAll_Should_Return_All_ToDos()
    {
        var toDos = new List<ToDoDto>
            {
                new ToDoDto { Id = 1, Title = "Task 1", ExpiryDate = DateTime.UtcNow.AddDays(1) },
                new ToDoDto { Id = 2, Title = "Task 2", ExpiryDate = DateTime.UtcNow.AddDays(2) }
            };

        _mockService.Setup(service => service.GetAllAsync())
                    .ReturnsAsync(toDos);

        var result = await _controller.GetAll();

        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult?.StatusCode.Should().Be(200);
        okResult?.Value.Should().BeEquivalentTo(toDos);
    }

    [Fact]
    public async Task Get_Should_Return_ToDo_When_Found()
    {
        var toDo = new ToDoDto { Id = 1, Title = "Task 1", ExpiryDate = DateTime.UtcNow.AddDays(1) };

        _mockService.Setup(service => service.GetByIdAsync(1))
                    .ReturnsAsync(toDo);

        var result = await _controller.Get(1);

        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult?.StatusCode.Should().Be(200);
        okResult?.Value.Should().BeEquivalentTo(toDo);
    }

    [Fact]
    public async Task Get_Should_Return_NotFound_When_ToDo_Not_Found()
    {
        _mockService.Setup(service => service.GetByIdAsync(999))
                    .ReturnsAsync(null as ToDoDto);

        var result = await _controller.Get(999);

        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task Update_Should_Return_NoContent_When_Successful()
    {
        var dto = new ToDoUpdateDto { Title = "Updated Task", Description = "Updated Desc", ExpiryDate = DateTime.UtcNow.AddDays(1) };

        _mockService.Setup(service => service.UpdateAsync(1, dto))
                    .ReturnsAsync(true);

        var result = await _controller.Update(1, dto);

        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task Update_Should_Return_NotFound_When_ToDo_Not_Found()
    {
        var dto = new ToDoUpdateDto { Title = "Updated Task", Description = "Updated Desc", ExpiryDate = DateTime.UtcNow.AddDays(1) };

        _mockService.Setup(service => service.UpdateAsync(999, dto))
                    .ReturnsAsync(false);

        var result = await _controller.Update(999, dto);

        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task SetComplete_Should_Return_NoContent_When_Successful()
    {
        _mockService.Setup(service => service.SetPercentCompleteAsync(1, 50))
                    .ReturnsAsync(true);

        var result = await _controller.SetComplete(1, 50);

        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task SetComplete_Should_Return_NotFound_When_ToDo_Not_Found()
    {
        _mockService.Setup(service => service.SetPercentCompleteAsync(999, 50))
                    .ReturnsAsync(false);

        var result = await _controller.SetComplete(999, 50);

        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task MarkAsDone_Should_Return_NoContent_When_Successful()
    {
        _mockService.Setup(service => service.MarkAsDoneAsync(1))
                    .ReturnsAsync(true);

        var result = await _controller.MarkAsDone(1);

        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task MarkAsDone_Should_Return_NotFound_When_ToDo_Not_Found()
    {
        _mockService.Setup(service => service.MarkAsDoneAsync(999))
                    .ReturnsAsync(false);

        var result = await _controller.MarkAsDone(999);

        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task Delete_Should_Return_NoContent_When_Successful()
    {
        _mockService.Setup(service => service.DeleteAsync(1))
                    .ReturnsAsync(true);

        var result = await _controller.Delete(1);

        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task Delete_Should_Return_NotFound_When_ToDo_Not_Found()
    {
        _mockService.Setup(service => service.DeleteAsync(999))
                    .ReturnsAsync(false);

        var result = await _controller.Delete(999);

        result.Should().BeOfType<NotFoundResult>();
    }
}