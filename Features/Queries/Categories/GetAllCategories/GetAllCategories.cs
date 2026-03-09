using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ToDoWebAPI.Data;

namespace ToDoWebAPI.Features.Queries.Categories.GetAllCategories;

public record GetAllCategoriesResponseDTO(int Id, string Name, string Description, string Color);

public class GetAllCategoriesHandler
{
    private readonly AppDbContext _context;
    private readonly ILogger<GetAllCategoriesHandler> _logger;

    public GetAllCategoriesHandler(AppDbContext context, ILogger<GetAllCategoriesHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<GetAllCategoriesResponseDTO>> HandleAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting all categories");

        var categories = await _context.Categories
            .Select(c => new GetAllCategoriesResponseDTO(c.Id, c.Name, c.Description, c.Color))
            .ToListAsync(cancellationToken);

        _logger.LogInformation("Retrieved {Count} categories", categories.Count);

        return categories;
    }
}
