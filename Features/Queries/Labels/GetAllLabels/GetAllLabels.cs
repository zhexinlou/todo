using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ToDoWebAPI.Data;

namespace ToDoWebAPI.Features.Queries.Labels.GetAllLabels;

// Query
public record GetAllLabelsQuery : IRequest<IEnumerable<LabelDto>>;

// Handler
public class GetAllLabelsHandler : IRequestHandler<GetAllLabelsQuery, IEnumerable<LabelDto>>
{
    private readonly AppDbContext _context;
    private readonly ILogger<GetAllLabelsHandler> _logger;

    public GetAllLabelsHandler(AppDbContext context, ILogger<GetAllLabelsHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<LabelDto>> Handle(GetAllLabelsQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting all labels");

        var labels = await _context.Labels
            .Select(l => new LabelDto(l.Id, l.Name))
            .ToListAsync(cancellationToken);

        _logger.LogInformation("Retrieved {Count} labels", labels.Count);

        return labels;
    }
}
