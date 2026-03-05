using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.Extensions.Logging;
using ToDoWebAPI.Data;
using ToDoWebAPI.Models;

namespace ToDoWebAPI.Features.Commands.Labels.CreateLabel;

// Request DTO
public record CreateLabelRequestDTO([Required] string Name);

// Response DTO
public record CreateLabelResponseDTO(int Id, string Name);

// Command
public record CreateLabelCommand([Required] string Name) : IRequest<CreateLabelResponseDTO>;

// Handler
public class CreateLabelHandler : IRequestHandler<CreateLabelCommand, CreateLabelResponseDTO>
{
    private readonly AppDbContext _context;
    private readonly ILogger<CreateLabelHandler> _logger;

    public CreateLabelHandler(AppDbContext context, ILogger<CreateLabelHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<CreateLabelResponseDTO> Handle(CreateLabelCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating label: {Name}", command.Name);

        var label = new Label
        {
            Name = command.Name
        };

        _context.Labels.Add(label);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Label created with Id: {Id}", label.Id);

        return new CreateLabelResponseDTO(label.Id, label.Name);
    }
}
