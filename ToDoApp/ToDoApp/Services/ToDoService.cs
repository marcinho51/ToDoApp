using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ToDoApp.Data;
using ToDoApp.Dtos;
using ToDoApp.Models;

namespace ToDoApp.Services;

public class ToDoService : IToDoService
{
    private readonly ToDoDbContext _context;
    private readonly IMapper _mapper;

    public ToDoService(ToDoDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<ToDoDto>> GetAllAsync()
    {
        var toDos = await _context.ToDos.ToListAsync();
        return _mapper.Map<List<ToDoDto>>(toDos);
    }

    public async Task<ToDoDto?> GetByIdAsync(int id)
    {
        var toDo = await _context.ToDos.FindAsync(id);
        return toDo is null ? null : _mapper.Map<ToDoDto>(toDo);
    }

    public async Task<List<ToDoDto>> GetIncomingAsync()
    {
        var now = DateTime.UtcNow;
        var endOfWeek = now.AddDays(7);

        var toDos = await _context.ToDos
            .Where(t => t.ExpiryDate.Date == now.Date ||
                        t.ExpiryDate.Date == now.AddDays(1).Date ||
                        (t.ExpiryDate >= now && t.ExpiryDate <= endOfWeek))
            .ToListAsync();

        return _mapper.Map<List<ToDoDto>>(toDos);
    }

    public async Task<ToDoDto> CreateAsync(ToDoCreateDto dto)
    {
        var toDo = _mapper.Map<ToDo>(dto);

        _context.ToDos.Add(toDo);
        await _context.SaveChangesAsync();

        return _mapper.Map<ToDoDto>(toDo);
    }

    public async Task<bool> UpdateAsync(int id, ToDoUpdateDto dto)
    {
        var existing = await _context.ToDos.FindAsync(id);

        if (existing is null) 
        { 
            return false; 
        }

        _mapper.Map(dto, existing);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> SetPercentCompleteAsync(int id, int percent)
    {
        var toDo = await _context.ToDos.FindAsync(id);

        if (toDo is null) 
        {
            return false;
        }
       
        toDo.PercentComplete = percent;
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> MarkAsDoneAsync(int id)
    {
        var toDo = await _context.ToDos.FindAsync(id);

        if (toDo is null)
        {
            return false;
        }

        toDo.PercentComplete = 100;
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var toDo = await _context.ToDos.FindAsync(id);

        if (toDo is null)
        {
            return false;
        }

        _context.ToDos.Remove(toDo);
        await _context.SaveChangesAsync();

        return true;
    }
}
