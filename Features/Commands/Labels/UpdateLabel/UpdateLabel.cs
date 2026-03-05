using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.Extensions.Logging;
using ToDoWebAPI.Data;

namespace ToDoWebAPI.Features.Commands.Labels.UpdateLabel;

// Request DTO
public record UpdateLabelRequestDTO([Required] int Id, [Required] string Name);

// Command
public record UpdateLabelCommand([Required] int Id, [Required] string Name) : IRequest<bool>;

// Handler
public class UpdateLabelHandler : IRequestHandler<UpdateLabelCommand, bool>
{
    private readonly AppDbContext _context;
    private readonly ILogger<UpdateLabelHandler> _logger;

    public UpdateLabelHandler(AppDbContext context, ILogger<UpdateLabelHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<bool> Handle(UpdateLabelCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating label Id: {Id}", command.Id);

        var label = await _context.Labels.FindAsync(new object[] { command.Id }, cancellationToken);
        if (label == null)
        {
            _logger.LogWarning("Label with Id {Id} not found", command.Id);
            return false;
        }

        label.Name = command.Name;
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Label {Id} updated successfully", command.Id);

        return true;
    }
}
