namespace ToDoApp.Dtos;

public class ToDoDto
{
    public int Id { get; set; }

    public string Title { get; set; } = default!;

    public string? Description { get; set; }

    public DateTime ExpiryDate { get; set; }

    public int PercentComplete { get; set; }
}
