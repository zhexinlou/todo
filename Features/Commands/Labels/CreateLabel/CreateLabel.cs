using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.Extensions.Logging;
using ToDoWebAPI.Data;
using ToDoWebAPI.Features.Queries.Labels;
using ToDoWebAPI.Models;

namespace ToDoWebAPI.Features.Commands.Labels.CreateLabel;

// Request DTO
public record CreateLabelRequest([Required] string Name);

// Command
public record CreateLabelCommand([Required] string Name) : IRequest<LabelDto>;

// Handler
public class CreateLabelHandler : IRequestHandler<CreateLabelCommand, LabelDto>
{
    private readonly AppDbContext _context;
    private readonly ILogger<CreateLabelHandler> _logger;

    public CreateLabelHandler(AppDbContext context, ILogger<CreateLabelHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<LabelDto> Handle(CreateLabelCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating label: {Name}", command.Name);

        var label = new Label
        {
            Name = command.Name
        };

        _context.Labels.Add(label);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Label created with Id: {Id}", label.Id);

        return new LabelDto(label.Id, label.Name);
    }
}
