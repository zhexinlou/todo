using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.Extensions.Logging;
using ToDoWebAPI.Data;

namespace ToDoWebAPI.Features.Commands.Categories.DeleteCategory;

// Command
public record DeleteCategoryCommand([Required] int Id) : IRequest<bool>;

// Handler
public class DeleteCategoryHandler : IRequestHandler<DeleteCategoryCommand, bool>
{
    private readonly AppDbContext _context;
    private readonly ILogger<DeleteCategoryHandler> _logger;

    public DeleteCategoryHandler(AppDbContext context, ILogger<DeleteCategoryHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<bool> Handle(DeleteCategoryCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deleting category Id: {Id}", command.Id);

        var category = await _context.Categories.FindAsync(new object[] { command.Id }, cancellationToken);
        if (category == null)
        {
            _logger.LogWarning("Category with Id {Id} not found", command.Id);
            return false;
        }

        _context.Categories.Remove(category);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Category {Id} deleted successfully", command.Id);

        return true;
    }
}
