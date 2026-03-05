using MediatR;
using Microsoft.AspNetCore.Mvc;
using ToDoWebAPI.Features.Commands.Labels.CreateLabel;
using ToDoWebAPI.Features.Commands.Labels.DeleteLabel;
using ToDoWebAPI.Features.Commands.Labels.UpdateLabel;
using ToDoWebAPI.Features.Queries.Labels.GetAllLabels;
using ToDoWebAPI.Features.Queries.Labels.GetLabelById;

namespace ToDoWebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LabelsController : ControllerBase
{
    private readonly IMediator _mediator;

    public LabelsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<GetAllLabelsResponseDTO>>> GetAll(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAllLabelsQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GetLabelByIdResponseDTO>> GetById(int id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetLabelByIdQuery(id), cancellationToken);

        if (result == null)
            return NotFound();

        return result;
    }

    [HttpPost]
    public async Task<ActionResult<CreateLabelResponseDTO>> Create(CreateLabelRequestDTO request, CancellationToken cancellationToken)
    {
        var command = new CreateLabelCommand(request.Name);
        var result = await _mediator.Send(command, cancellationToken);

        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateLabelRequestDTO request, CancellationToken cancellationToken)
    {
        if (id != request.Id)
            return BadRequest();

        var command = new UpdateLabelCommand(request.Id, request.Name);
        var success = await _mediator.Send(command, cancellationToken);

        if (!success)
            return NotFound();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var command = new DeleteLabelCommand(id);
        var success = await _mediator.Send(command, cancellationToken);

        if (!success)
            return NotFound();

        return NoContent();
    }
}
