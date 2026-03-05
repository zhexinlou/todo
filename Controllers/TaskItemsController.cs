using MediatR;
using Microsoft.AspNetCore.Mvc;
using ToDoWebAPI.Features.Commands.TaskItems.CreateTaskItem;
using ToDoWebAPI.Features.Commands.TaskItems.DeleteTaskItem;
using ToDoWebAPI.Features.Commands.TaskItems.UpdateTaskItem;
using ToDoWebAPI.Features.Queries.TaskItems.GetAllTaskItems;
using ToDoWebAPI.Features.Queries.TaskItems.GetTaskItemById;

namespace ToDoWebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TaskItemsController : ControllerBase
{
    private readonly IMediator _mediator;

    public TaskItemsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<GetAllTaskItemsResponseDTO>>> GetAll(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAllTaskItemsQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GetTaskItemByIdResponseDTO>> GetById(int id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetTaskItemByIdQuery(id), cancellationToken);

        if (result == null)
            return NotFound();

        return result;
    }

    [HttpPost]
    public async Task<ActionResult<CreateTaskItemResponseDTO>> Create(CreateTaskItemRequestDTO request, CancellationToken cancellationToken)
    {
        var command = new CreateTaskItemCommand(request.Title, request.Description, request.DueDate, request.LabelId);
        var result = await _mediator.Send(command, cancellationToken);

        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateTaskItemRequestDTO request, CancellationToken cancellationToken)
    {
        if (id != request.Id)
            return BadRequest();

        var command = new UpdateTaskItemCommand(request.Id, request.Title, request.Description, request.DueDate, request.LabelId);
        var success = await _mediator.Send(command, cancellationToken);

        if (!success)
            return NotFound();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var command = new DeleteTaskItemCommand(id);
        var success = await _mediator.Send(command, cancellationToken);

        if (!success)
            return NotFound();

        return NoContent();
    }
}
