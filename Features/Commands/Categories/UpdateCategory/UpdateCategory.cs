using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.Extensions.Logging;
using ToDoWebAPI.Data;

namespace ToDoWebAPI.Features.Commands.Categories.UpdateCategory;

// Request DTO
public record UpdateCategoryRequest(
    [Required] int Id,
    [Required] string Name,
    string Description,
    string Color
);

// Command
public record UpdateCategoryCommand(
    [Required] int Id,
    [Required] string Name,
    string Description,
    string Color
) : IRequest<bool>;

// Handler
public class UpdateCategoryHandler : IRequestHandler<UpdateCategoryCommand, bool>
{
    private readonly AppDbContext _context;
    private readonly ILogger<UpdateCategoryHandler> _logger;

    public UpdateCategoryHandler(AppDbContext context, ILogger<UpdateCategoryHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<bool> Handle(UpdateCategoryCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating category Id: {Id}", command.Id);

        var category = await _context.Categories.FindAsync(new object[] { command.Id }, cancellationToken);
        if (category == null)
        {
            _logger.LogWarning("Category with Id {Id} not found", command.Id);
            return false;
        }

        category.Name = command.Name;
        category.Description = command.Description;
        category.Color = command.Color;

        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Category {Id} updated successfully", command.Id);

        return true;
    }
}
