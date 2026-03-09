using Microsoft.Extensions.Logging;
using ToDoWebAPI.Data;

namespace ToDoWebAPI.Features.Commands.Categories.DeleteCategory;

public class DeleteCategoryHandler
{
    private readonly AppDbContext _context;
    private readonly ILogger<DeleteCategoryHandler> _logger;

    public DeleteCategoryHandler(AppDbContext context, ILogger<DeleteCategoryHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task HandleAsync(int id, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deleting category Id: {Id}", id);

        var category = await _context.Categories.FindAsync(new object[] { id }, cancellationToken)
            ?? throw new Exception($"Category {id} not found");

        _context.Categories.Remove(category);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Category {Id} deleted successfully", id);
    }
}
