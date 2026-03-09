using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ToDoWebAPI.Data;

namespace ToDoWebAPI.Features.Queries.Categories.GetCategoryById;

public record GetCategoryByIdResponseDTO(int Id, string Name, string Description, string Color);

public class GetCategoryByIdHandler
{
    private readonly AppDbContext _context;
    private readonly ILogger<GetCategoryByIdHandler> _logger;

    public GetCategoryByIdHandler(AppDbContext context, ILogger<GetCategoryByIdHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<GetCategoryByIdResponseDTO> HandleAsync(int id, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting category by Id: {Id}", id);

        var category = await _context.Categories
            .Where(c => c.Id == id)
            .Select(c => new GetCategoryByIdResponseDTO(c.Id, c.Name, c.Description, c.Color))
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new Exception($"Category {id} not found");

        _logger.LogInformation("Retrieved category: {Name}", category.Name);

        return category;
    }
}
