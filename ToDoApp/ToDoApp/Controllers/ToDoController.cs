using Microsoft.AspNetCore.Mvc;
using ToDoApp.Dtos;
using ToDoApp.Services;

namespace ToDoApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ToDoController : ControllerBase
{
    private readonly IToDoService _service;

    public ToDoController(IToDoService service)
    {
        _service = service;
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<ToDoDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ToDoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(int id)
    {
        var toDo = await _service.GetByIdAsync(id);
        return toDo == null ? NotFound() : Ok(toDo);
    }

    [HttpGet("incoming")]
    [ProducesResponseType(typeof(List<ToDoDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetIncoming() => Ok(await _service.GetIncomingAsync());

    [HttpPost]
    [ProducesResponseType(typeof(ToDoDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] ToDoCreateDto dto)
    {
        var created = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] ToDoUpdateDto dto)
    {
        if (!await _service.UpdateAsync(id, dto))
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpPatch("{id}/complete")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> SetComplete(int id, [FromQuery] int percent)
    {
        if (!await _service.SetPercentCompleteAsync(id, percent))
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpPatch("{id}/done")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> MarkAsDone(int id)
    {
        if (!await _service.MarkAsDoneAsync(id))
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        if (!await _service.DeleteAsync(id))
        {
            return NotFound();
        }

        return NoContent();
    }
}