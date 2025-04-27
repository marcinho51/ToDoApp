using System.ComponentModel.DataAnnotations;

namespace ToDoApp.Dtos;

public class ToDoUpdateDto
{
    [Required]
    public string Title { get; set; } = default!;

    public string? Description { get; set; }

    [Required]
    public DateTime ExpiryDate { get; set; }

    [Range(0, 100)]
    public int PercentComplete { get; set; }
}
