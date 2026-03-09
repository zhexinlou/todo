using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ToDoWebAPI.Data;

namespace ToDoWebAPI.Features.Queries.TaskItems.GetTaskItemById;

public record GetTaskItemByIdResponseDTO(
    int Id,
    string Title,
    string Description,
    DateTime DueDate,
    int? CategoryId,
    string? CategoryName
);

public class GetTaskItemByIdHandler
{
    private readonly AppDbContext _context;
    private readonly ILogger<GetTaskItemByIdHandler> _logger;

    public GetTaskItemByIdHandler(AppDbContext context, ILogger<GetTaskItemByIdHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<GetTaskItemByIdResponseDTO> HandleAsync(int id, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting task item by Id: {Id}", id);

        var taskItem = await _context.TaskItems
            .Where(t => t.Id == id)
            .Select(t => new GetTaskItemByIdResponseDTO(
                t.Id,
                t.Title,
                t.Description,
                t.DueDate,
                t.CategoryId,
                t.Category != null ? t.Category.Name : null
            ))
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new Exception($"TaskItem {id} not found");

        _logger.LogInformation("Retrieved task item: {Title}", taskItem.Title);

        return taskItem;
    }
}
