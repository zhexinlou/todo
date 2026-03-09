using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Logging;
using ToDoWebAPI.Data;
using ToDoWebAPI.Models;

namespace ToDoWebAPI.Features.Commands.TaskItems.CreateTaskItem;

public record CreateTaskItemRequestDTO(
    [Required] string Title,
    string Description,
    [Required] DateTime DueDate,
    int? CategoryId
);

public class CreateTaskItemHandler
{
    private readonly AppDbContext _context;
    private readonly ILogger<CreateTaskItemHandler> _logger;

    public CreateTaskItemHandler(AppDbContext context, ILogger<CreateTaskItemHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<int> HandleAsync(CreateTaskItemRequestDTO request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating task item: {Title}", request.Title);

        var taskItem = new TaskItem
        {
            Title = request.Title,
            Description = request.Description,
            DueDate = request.DueDate,
            CategoryId = request.CategoryId
        };

        _context.TaskItems.Add(taskItem);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Task item created with Id: {Id}", taskItem.Id);

        return taskItem.Id;
    }
}
