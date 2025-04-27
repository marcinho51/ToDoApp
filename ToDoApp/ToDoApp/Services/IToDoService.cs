using ToDoApp.Dtos;

namespace ToDoApp.Services;

public interface IToDoService
{
    Task<List<ToDoDto>> GetAllAsync();

    Task<ToDoDto?> GetByIdAsync(int id);

    Task<List<ToDoDto>> GetIncomingAsync();

    Task<ToDoDto> CreateAsync(ToDoCreateDto dto);

    Task<bool> UpdateAsync(int id, ToDoUpdateDto dto);

    Task<bool> SetPercentCompleteAsync(int id, int percent);

    Task<bool> MarkAsDoneAsync(int id);

    Task<bool> DeleteAsync(int id);
}
