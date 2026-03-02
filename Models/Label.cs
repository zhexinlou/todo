namespace ToDoWebAPI.Models;

public class Label
{
    public int Id { get; set; }
    public string Name { get; set; } = String.Empty;
    public List<TaskItem> TaskItems { get; set; } = new();
}