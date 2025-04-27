using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ToDoApp.Data;
using ToDoApp.Dtos;
using ToDoApp.Models;
using ToDoApp.Services;
using Xunit;

namespace ToDoApp.Tests.Services;

public class ToDoServiceTests
{
    private readonly IToDoService _service;
    private readonly ToDoDbContext _context;
    private readonly IMapper _mapper;

    public ToDoServiceTests()
    {
        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<ToDoCreateDto, ToDo>();
            cfg.CreateMap<ToDoUpdateDto, ToDo>();
            cfg.CreateMap<ToDo, ToDoDto>();
        });

        _mapper = mapperConfig.CreateMapper();

        var options = new DbContextOptionsBuilder<ToDoDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new ToDoDbContext(options);
        _service = new ToDoService(_context, _mapper);
    }

    [Fact]
    public async Task CreateAsync_Should_Add_ToDo()
    {
        var dto = new ToDoCreateDto
        {
            Title = "Test Task",
            Description = "desc",
            ExpiryDate = DateTime.UtcNow.AddDays(1)
        };

        var result = await _service.CreateAsync(dto);

        Assert.NotNull(result);
        Assert.Equal("Test Task", result.Title);
        Assert.Single(_context.ToDos);
    }

    [Fact]
    public async Task GetAllAsync_Should_Return_All_ToDos()
    {
        var toDo1 = new ToDo { Title = "Task 1", ExpiryDate = DateTime.UtcNow.AddDays(1), Description = "Desc" };
        var toDo2 = new ToDo { Title = "Task 2", ExpiryDate = DateTime.UtcNow.AddDays(2), Description = "Desc" };
        _context.ToDos.AddRange(toDo1, toDo2);
        await _context.SaveChangesAsync();

        var toDos = await _service.GetAllAsync();

        Assert.Equal(2, toDos.Count);
    }

    [Fact]
    public async Task GetByIdAsync_Should_Return_ToDo_When_Found()
    {
        var toDo = new ToDo { Title = "Test Task", ExpiryDate = DateTime.UtcNow.AddDays(1), Description = "Desc" };
        _context.ToDos.Add(toDo);
        await _context.SaveChangesAsync();

        var result = await _service.GetByIdAsync(toDo.Id);

        Assert.NotNull(result);
        Assert.Equal("Test Task", result.Title);
    }

    [Fact]
    public async Task GetByIdAsync_Should_Return_Null_When_Not_Found()
    {
        var result = await _service.GetByIdAsync(999);
        Assert.Null(result);
    }

    [Fact]
    public async Task GetIncomingAsync_Should_Return_Incoming_ToDos()
    {
        var toDo = new ToDo
        {
            Title = "Upcoming Task",
            ExpiryDate = DateTime.UtcNow.AddDays(1),
            Description = "Desc"
        };
        _context.ToDos.Add(toDo);
        await _context.SaveChangesAsync();

        var result = await _service.GetIncomingAsync();

        Assert.Single(result);
        Assert.Equal("Upcoming Task", result[0].Title);
    }

    [Fact]
    public async Task UpdateAsync_Should_Update_Existing_ToDo()
    {
        var toDo = new ToDo { Title = "Old Task", ExpiryDate = DateTime.UtcNow.AddDays(1), Description = "Desc" };
        _context.ToDos.Add(toDo);
        await _context.SaveChangesAsync();

        var dto = new ToDoUpdateDto
        {
            Title = "Updated Task",
            Description = "Updated Description",
            ExpiryDate = DateTime.UtcNow.AddDays(2),
            PercentComplete = 50
        };

        var result = await _service.UpdateAsync(toDo.Id, dto);

        Assert.True(result);
        var updatedToDo = await _context.ToDos.FindAsync(toDo.Id);
        Assert.Equal("Updated Task", updatedToDo?.Title);
        Assert.Equal("Updated Description", updatedToDo?.Description);
    }

    [Fact]
    public async Task SetPercentCompleteAsync_Should_Update_PercentComplete()
    {
        var toDo = new ToDo { Title = "Task", ExpiryDate = DateTime.UtcNow.AddDays(1), PercentComplete = 0, Description = "Desc" };
        _context.ToDos.Add(toDo);
        await _context.SaveChangesAsync();

        var result = await _service.SetPercentCompleteAsync(toDo.Id, 50);

        Assert.True(result);
        var updatedToDo = await _context.ToDos.FindAsync(toDo.Id);
        Assert.Equal(50, updatedToDo?.PercentComplete);
    }

    [Fact]
    public async Task MarkAsDoneAsync_Should_Set_PercentComplete_To_100()
    {
        var toDo = new ToDo { Title = "Task", ExpiryDate = DateTime.UtcNow.AddDays(1), PercentComplete = 50, Description = "Desc" };
        _context.ToDos.Add(toDo);
        await _context.SaveChangesAsync();

        var result = await _service.MarkAsDoneAsync(toDo.Id);

        Assert.True(result);
        var updatedToDo = await _context.ToDos.FindAsync(toDo.Id);
        Assert.Equal(100, updatedToDo?.PercentComplete);
    }

    [Fact]
    public async Task DeleteAsync_Should_Remove_ToDo()
    {
        var toDo = new ToDo { Title = "Task", ExpiryDate = DateTime.UtcNow.AddDays(1), Description = "Desc" };
        _context.ToDos.Add(toDo);
        await _context.SaveChangesAsync();

        var result = await _service.DeleteAsync(toDo.Id);

        Assert.True(result);
        var deletedToDo = await _context.ToDos.FindAsync(toDo.Id);
        Assert.Null(deletedToDo);
    }
}

