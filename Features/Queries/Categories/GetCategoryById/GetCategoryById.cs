using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ToDoWebAPI.Data;

namespace ToDoWebAPI.Features.Queries.Categories.GetCategoryById;

// Query
public record GetCategoryByIdQuery([Required] int Id) : IRequest<CategoryDto?>;

// Handler
public class GetCategoryByIdHandler : IRequestHandler<GetCategoryByIdQuery, CategoryDto?>
{
    private readonly AppDbContext _context;
    private readonly ILogger<GetCategoryByIdHandler> _logger;

    public GetCategoryByIdHandler(AppDbContext context, ILogger<GetCategoryByIdHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<CategoryDto?> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting category by Id: {Id}", request.Id);

        var category = await _context.Categories
            .Where(c => c.Id == request.Id)
            .Select(c => new CategoryDto(c.Id, c.Name, c.Description, c.Color))
            .FirstOrDefaultAsync(cancellationToken);

        if (category == null)
        {
            _logger.LogWarning("Category with Id {Id} not found", request.Id);
        }
        else
        {
            _logger.LogInformation("Retrieved category: {Name}", category.Name);
        }

        return category;
    }
}
