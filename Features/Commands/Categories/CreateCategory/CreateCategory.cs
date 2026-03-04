using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.Extensions.Logging;
using ToDoWebAPI.Data;
using ToDoWebAPI.Features.Queries.Categories;
using ToDoWebAPI.Models;

namespace ToDoWebAPI.Features.Commands.Categories.CreateCategory;

// Request DTO
public record CreateCategoryRequest(
    [Required] string Name,
    string Description,
    string Color
);

// Command
public record CreateCategoryCommand(
    [Required] string Name,
    string Description,
    string Color
) : IRequest<CategoryDto>;

// Handler
public class CreateCategoryHandler : IRequestHandler<CreateCategoryCommand, CategoryDto>
{
    private readonly AppDbContext _context;
    private readonly ILogger<CreateCategoryHandler> _logger;

    public CreateCategoryHandler(AppDbContext context, ILogger<CreateCategoryHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<CategoryDto> Handle(CreateCategoryCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating category: {Name}", command.Name);

        var category = new Category
        {
            Name = command.Name,
            Description = command.Description,
            Color = command.Color
        };

        _context.Categories.Add(category);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Category created with Id: {Id}", category.Id);

        return new CategoryDto(category.Id, category.Name, category.Description, category.Color);
    }
}
