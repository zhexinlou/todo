using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ToDoWebAPI.Data;

namespace ToDoWebAPI.Features.Queries.TaskItems.GetAllTaskItems;

// Response DTO
public record GetAllTaskItemsResponseDTO(
    int Id,
    string Title,
    string Description,
    DateTime DueDate,
    int? LabelId,
    string? LabelName
);

// Query
public record GetAllTaskItemsQuery : IRequest<IEnumerable<GetAllTaskItemsResponseDTO>>;

// Handler
public class GetAllTaskItemsHandler : IRequestHandler<GetAllTaskItemsQuery, IEnumerable<GetAllTaskItemsResponseDTO>>
{
    private readonly AppDbContext _context;
    private readonly ILogger<GetAllTaskItemsHandler> _logger;

    public GetAllTaskItemsHandler(AppDbContext context, ILogger<GetAllTaskItemsHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<GetAllTaskItemsResponseDTO>> Handle(GetAllTaskItemsQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting all task items");

        var taskItems = await _context.TaskItems
            .Include(t => t.Label)
            .Select(t => new GetAllTaskItemsResponseDTO(
                t.Id,
                t.Title,
                t.Description,
                t.DueDate,
                t.LabelId,
                t.Label != null ? t.Label.Name : null
            ))
            .ToListAsync(cancellationToken);

        _logger.LogInformation("Retrieved {Count} task items", taskItems.Count);

        return taskItems;
    }
}
