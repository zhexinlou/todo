namespace ToDoWebAPI.Models;

public class TaskItem
{
    public int Id { get; set; }
    public int? LabelId { get; set; }
    public Label? Label { get; set; }
    public string Description { get; set; } = String.Empty;
    public string Title { get; set; } = String.Empty;
    public DateTime DueDate { get; set; } = DateTime.UtcNow;

    public int? CategoryId { get; set; } = 1;
    public Category? Category { get; set; }
}