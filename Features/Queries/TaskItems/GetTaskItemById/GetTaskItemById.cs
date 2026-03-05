using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ToDoWebAPI.Data;

namespace ToDoWebAPI.Features.Queries.TaskItems.GetTaskItemById;

// Response DTO
public record GetTaskItemByIdResponseDTO(
    int Id,
    string Title,
    string Description,
    DateTime DueDate,
    int? LabelId,
    string? LabelName
);

// Query
public record GetTaskItemByIdQuery([Required] int Id) : IRequest<GetTaskItemByIdResponseDTO?>;

// Handler
public class GetTaskItemByIdHandler : IRequestHandler<GetTaskItemByIdQuery, GetTaskItemByIdResponseDTO?>
{
    private readonly AppDbContext _context;
    private readonly ILogger<GetTaskItemByIdHandler> _logger;

    public GetTaskItemByIdHandler(AppDbContext context, ILogger<GetTaskItemByIdHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<GetTaskItemByIdResponseDTO?> Handle(GetTaskItemByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting task item by Id: {Id}", request.Id);

        var taskItem = await _context.TaskItems
            .Include(t => t.Label)
            .Where(t => t.Id == request.Id)
            .Select(t => new GetTaskItemByIdResponseDTO(
                t.Id,
                t.Title,
                t.Description,
                t.DueDate,
                t.LabelId,
                t.Label != null ? t.Label.Name : null
            ))
            .FirstOrDefaultAsync(cancellationToken);

        if (taskItem == null)
        {
            _logger.LogWarning("Task item with Id {Id} not found", request.Id);
        }
        else
        {
            _logger.LogInformation("Retrieved task item: {Title}", taskItem.Title);
        }

        return taskItem;
    }
}
