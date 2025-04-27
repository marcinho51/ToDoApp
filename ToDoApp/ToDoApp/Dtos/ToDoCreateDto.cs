using System.ComponentModel.DataAnnotations;

namespace ToDoApp.Dtos;

public class ToDoCreateDto
{
    [Required]
    public string Title { get; set; } = default!;

    public string? Description { get; set; }

    [Required]
    public DateTime ExpiryDate { get; set; }
}
