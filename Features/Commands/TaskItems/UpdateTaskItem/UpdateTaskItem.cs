using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.Extensions.Logging;
using ToDoWebAPI.Data;

namespace ToDoWebAPI.Features.Commands.TaskItems.UpdateTaskItem;

// Request DTO
public record UpdateTaskItemRequestDTO(
    [Required] int Id,
    [Required] string Title,
    string Description,
    [Required] DateTime DueDate,
    int? LabelId
);

// Command
public record UpdateTaskItemCommand(
    [Required] int Id,
    [Required] string Title,
    string Description,
    [Required] DateTime DueDate,
    int? LabelId
) : IRequest<bool>;

// Handler
public class UpdateTaskItemHandler : IRequestHandler<UpdateTaskItemCommand, bool>
{
    private readonly AppDbContext _context;
    private readonly ILogger<UpdateTaskItemHandler> _logger;

    public UpdateTaskItemHandler(AppDbContext context, ILogger<UpdateTaskItemHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<bool> Handle(UpdateTaskItemCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating task item Id: {Id}", command.Id);

        var taskItem = await _context.TaskItems.FindAsync(new object[] { command.Id }, cancellationToken);
        if (taskItem == null)
        {
            _logger.LogWarning("Task item with Id {Id} not found", command.Id);
            return false;
        }

        taskItem.Title = command.Title;
        taskItem.Description = command.Description;
        taskItem.DueDate = command.DueDate;
        taskItem.LabelId = command.LabelId;

        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Task item {Id} updated successfully", command.Id);

        return true;
    }
}
