using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Logging;
using ToDoWebAPI.Data;

namespace ToDoWebAPI.Features.Commands.Categories.UpdateCategory;

public record UpdateCategoryRequestDTO(
    [Required] int Id,
    [Required] string Name,
    string Description,
    string Color
);

public class UpdateCategoryHandler
{
    private readonly AppDbContext _context;
    private readonly ILogger<UpdateCategoryHandler> _logger;

    public UpdateCategoryHandler(AppDbContext context, ILogger<UpdateCategoryHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task HandleAsync(UpdateCategoryRequestDTO request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating category Id: {Id}", request.Id);

        var category = await _context.Categories.FindAsync(new object[] { request.Id }, cancellationToken)
            ?? throw new Exception($"Category {request.Id} not found");

        category.Name = request.Name;
        category.Description = request.Description;
        category.Color = request.Color;

        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Category {Id} updated successfully", request.Id);
    }
}
