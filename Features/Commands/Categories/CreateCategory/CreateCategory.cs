using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.Extensions.Logging;
using ToDoWebAPI.Data;
using ToDoWebAPI.Models;

namespace ToDoWebAPI.Features.Commands.Categories.CreateCategory;

// Request DTO
public record CreateCategoryRequestDTO(
    [Required] string Name,
    string Description,
    string Color
);

// Response DTO
public record CreateCategoryResponseDTO(int Id, string Name, string Description, string Color);

// Command
public record CreateCategoryCommand(
    [Required] string Name,
    string Description,
    string Color
) : IRequest<CreateCategoryResponseDTO>;

// Handler
public class CreateCategoryHandler : IRequestHandler<CreateCategoryCommand, CreateCategoryResponseDTO>
{
    private readonly AppDbContext _context;
    private readonly ILogger<CreateCategoryHandler> _logger;

    public CreateCategoryHandler(AppDbContext context, ILogger<CreateCategoryHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<CreateCategoryResponseDTO> Handle(CreateCategoryCommand command, CancellationToken cancellationToken)
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

        return new CreateCategoryResponseDTO(category.Id, category.Name, category.Description, category.Color);
    }
}
