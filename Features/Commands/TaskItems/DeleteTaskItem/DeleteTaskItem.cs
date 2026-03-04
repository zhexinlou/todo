using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.Extensions.Logging;
using ToDoWebAPI.Data;

namespace ToDoWebAPI.Features.Commands.TaskItems.DeleteTaskItem;

// Command
public record DeleteTaskItemCommand([Required] int Id) : IRequest<bool>;

// Handler
public class DeleteTaskItemHandler : IRequestHandler<DeleteTaskItemCommand, bool>
{
    private readonly AppDbContext _context;
    private readonly ILogger<DeleteTaskItemHandler> _logger;

    public DeleteTaskItemHandler(AppDbContext context, ILogger<DeleteTaskItemHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<bool> Handle(DeleteTaskItemCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deleting task item Id: {Id}", command.Id);

        var taskItem = await _context.TaskItems.FindAsync(new object[] { command.Id }, cancellationToken);
        if (taskItem == null)
        {
            _logger.LogWarning("Task item with Id {Id} not found", command.Id);
            return false;
        }

        _context.TaskItems.Remove(taskItem);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Task item {Id} deleted successfully", command.Id);

        return true;
    }
}
