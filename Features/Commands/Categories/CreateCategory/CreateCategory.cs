using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Logging;
using ToDoWebAPI.Data;
using ToDoWebAPI.Models;

namespace ToDoWebAPI.Features.Commands.Categories.CreateCategory;

public record CreateCategoryRequestDTO(
    [Required] string Name,
    string Description,
    string Color
);

public class CreateCategoryHandler
{
    private readonly AppDbContext _context;
    private readonly ILogger<CreateCategoryHandler> _logger;

    public CreateCategoryHandler(AppDbContext context, ILogger<CreateCategoryHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<int> HandleAsync(CreateCategoryRequestDTO request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating category: {Name}", request.Name);

        var category = new Category
        {
            Name = request.Name,
            Description = request.Description,
            Color = request.Color
        };

        _context.Categories.Add(category);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Category created with Id: {Id}", category.Id);

        return category.Id;
    }
}
