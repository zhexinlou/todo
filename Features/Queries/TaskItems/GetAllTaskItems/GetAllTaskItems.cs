using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ToDoWebAPI.Data;

namespace ToDoWebAPI.Features.Queries.TaskItems.GetAllTaskItems;

public record GetAllTaskItemsResponseDTO(
    int Id,
    string Title,
    string Description,
    DateTime DueDate,
    int? CategoryId,
    string? CategoryName
);

public class GetAllTaskItemsHandler
{
    private readonly AppDbContext _context;
    private readonly ILogger<GetAllTaskItemsHandler> _logger;

    public GetAllTaskItemsHandler(AppDbContext context, ILogger<GetAllTaskItemsHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<GetAllTaskItemsResponseDTO>> HandleAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting all task items");

        var taskItems = await _context.TaskItems
            .Select(t => new GetAllTaskItemsResponseDTO(
                t.Id,
                t.Title,
                t.Description,
                t.DueDate,
                t.CategoryId,
                t.Category != null ? t.Category.Name : null
            ))
            .ToListAsync(cancellationToken);

        _logger.LogInformation("Retrieved {Count} task items", taskItems.Count);

        return taskItems;
    }
}
