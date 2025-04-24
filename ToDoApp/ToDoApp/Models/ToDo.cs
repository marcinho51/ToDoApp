namespace ToDoApp.Models;
public class ToDo
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime ExpiryDate { get; set; }
    public int PercentComplete { get; set; }
}
