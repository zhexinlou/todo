using Microsoft.Extensions.Logging;
using ToDoWebAPI.Data;

namespace ToDoWebAPI.Features.Commands.TaskItems.DeleteTaskItem;

public class DeleteTaskItemHandler
{
    private readonly AppDbContext _context;
    private readonly ILogger<DeleteTaskItemHandler> _logger;

    public DeleteTaskItemHandler(AppDbContext context, ILogger<DeleteTaskItemHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task HandleAsync(int id, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deleting task item Id: {Id}", id);

        var taskItem = await _context.TaskItems.FindAsync(new object[] { id }, cancellationToken)
            ?? throw new Exception($"TaskItem {id} not found");

        _context.TaskItems.Remove(taskItem);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Task item {Id} deleted successfully", id);
    }
}
