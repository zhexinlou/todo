using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Logging;
using ToDoWebAPI.Data;

namespace ToDoWebAPI.Features.Commands.TaskItems.UpdateTaskItem;

public record UpdateTaskItemRequestDTO(
    [Required] int Id,
    [Required] string Title,
    string Description,
    [Required] DateTime DueDate,
    int? CategoryId
);

public class UpdateTaskItemHandler
{
    private readonly AppDbContext _context;
    private readonly ILogger<UpdateTaskItemHandler> _logger;

    public UpdateTaskItemHandler(AppDbContext context, ILogger<UpdateTaskItemHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task HandleAsync(UpdateTaskItemRequestDTO request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating task item Id: {Id}", request.Id);

        var taskItem = await _context.TaskItems.FindAsync(new object[] { request.Id }, cancellationToken)
            ?? throw new Exception($"TaskItem {request.Id} not found");

        taskItem.Title = request.Title;
        taskItem.Description = request.Description;
        taskItem.DueDate = request.DueDate;
        taskItem.CategoryId = request.CategoryId;

        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Task item {Id} updated successfully", request.Id);
    }
}
