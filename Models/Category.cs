namespace ToDoWebAPI.Models;

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; } = String.Empty;
    public string Description { get; set; } = String.Empty;
    public string Color { get; set; } = String.Empty;
    public List<TaskItem> TaskItems { get; set; } = new();
}