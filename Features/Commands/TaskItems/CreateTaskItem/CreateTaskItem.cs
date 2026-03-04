using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.Extensions.Logging;
using ToDoWebAPI.Data;
using ToDoWebAPI.Features.Queries.TaskItems;
using ToDoWebAPI.Models;

namespace ToDoWebAPI.Features.Commands.TaskItems.CreateTaskItem;

// Request DTO
public record CreateTaskItemRequest(
    [Required] string Title,
    string Description,
    [Required] DateTime DueDate,
    int? LabelId
);

// Command
public record CreateTaskItemCommand(
    [Required] string Title,
    string Description,
    [Required] DateTime DueDate,
    int? LabelId
) : IRequest<TaskItemDto>;

// Handler
public class CreateTaskItemHandler : IRequestHandler<CreateTaskItemCommand, TaskItemDto>
{
    private readonly AppDbContext _context;
    private readonly ILogger<CreateTaskItemHandler> _logger;

    public CreateTaskItemHandler(AppDbContext context, ILogger<CreateTaskItemHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<TaskItemDto> Handle(CreateTaskItemCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating task item: {Title}", command.Title);

        var taskItem = new TaskItem
        {
            Title = command.Title,
            Description = command.Description,
            DueDate = command.DueDate,
            LabelId = command.LabelId
        };

        _context.TaskItems.Add(taskItem);
        await _context.SaveChangesAsync(cancellationToken);

        if (taskItem.LabelId.HasValue)
        {
            await _context.Entry(taskItem).Reference(t => t.Label).LoadAsync(cancellationToken);
        }

        _logger.LogInformation("Task item created with Id: {Id}", taskItem.Id);

        return new TaskItemDto(
            taskItem.Id,
            taskItem.Title,
            taskItem.Description,
            taskItem.DueDate,
            taskItem.LabelId,
            taskItem.Label?.Name
        );
    }
}
