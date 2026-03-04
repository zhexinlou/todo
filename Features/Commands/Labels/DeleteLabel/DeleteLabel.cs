using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.Extensions.Logging;
using ToDoWebAPI.Data;

namespace ToDoWebAPI.Features.Commands.Labels.DeleteLabel;

// Command
public record DeleteLabelCommand([Required] int Id) : IRequest<bool>;

// Handler
public class DeleteLabelHandler : IRequestHandler<DeleteLabelCommand, bool>
{
    private readonly AppDbContext _context;
    private readonly ILogger<DeleteLabelHandler> _logger;

    public DeleteLabelHandler(AppDbContext context, ILogger<DeleteLabelHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<bool> Handle(DeleteLabelCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deleting label Id: {Id}", command.Id);

        var label = await _context.Labels.FindAsync(new object[] { command.Id }, cancellationToken);
        if (label == null)
        {
            _logger.LogWarning("Label with Id {Id} not found", command.Id);
            return false;
        }

        _context.Labels.Remove(label);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Label {Id} deleted successfully", command.Id);

        return true;
    }
}
