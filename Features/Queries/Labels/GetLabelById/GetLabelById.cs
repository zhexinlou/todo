using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ToDoWebAPI.Data;

namespace ToDoWebAPI.Features.Queries.Labels.GetLabelById;

// Response DTO
public record GetLabelByIdResponseDTO(int Id, string Name);

// Query
public record GetLabelByIdQuery([Required] int Id) : IRequest<GetLabelByIdResponseDTO?>;

// Handler
public class GetLabelByIdHandler : IRequestHandler<GetLabelByIdQuery, GetLabelByIdResponseDTO?>
{
    private readonly AppDbContext _context;
    private readonly ILogger<GetLabelByIdHandler> _logger;

    public GetLabelByIdHandler(AppDbContext context, ILogger<GetLabelByIdHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<GetLabelByIdResponseDTO?> Handle(GetLabelByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting label by Id: {Id}", request.Id);

        var label = await _context.Labels
            .Where(l => l.Id == request.Id)
            .Select(l => new GetLabelByIdResponseDTO(l.Id, l.Name))
            .FirstOrDefaultAsync(cancellationToken);

        if (label == null)
        {
            _logger.LogWarning("Label with Id {Id} not found", request.Id);
        }
        else
        {
            _logger.LogInformation("Retrieved label: {Name}", label.Name);
        }

        return label;
    }
}
