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
    private readonly CreateTaskItemHandler _createHandler;
    private readonly UpdateTaskItemHandler _updateHandler;
    private readonly DeleteTaskItemHandler _deleteHandler;
    private readonly GetAllTaskItemsHandler _getAllHandler;
    private readonly GetTaskItemByIdHandler _getByIdHandler;

    public TaskItemsController(
        CreateTaskItemHandler createHandler,
        UpdateTaskItemHandler updateHandler,
        DeleteTaskItemHandler deleteHandler,
        GetAllTaskItemsHandler getAllHandler,
        GetTaskItemByIdHandler getByIdHandler)
    {
        _createHandler = createHandler;
        _updateHandler = updateHandler;
        _deleteHandler = deleteHandler;
        _getAllHandler = getAllHandler;
        _getByIdHandler = getByIdHandler;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<GetAllTaskItemsResponseDTO>>> GetAll(CancellationToken cancellationToken)
    {
        var result = await _getAllHandler.HandleAsync(cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GetTaskItemByIdResponseDTO>> GetById(int id, CancellationToken cancellationToken)
    {
        var result = await _getByIdHandler.HandleAsync(id, cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<int>> Create(CreateTaskItemRequestDTO request, CancellationToken cancellationToken)
    {
        var id = await _createHandler.HandleAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateTaskItemRequestDTO request, CancellationToken cancellationToken)
    {
        if (id != request.Id)
            return BadRequest();

        await _updateHandler.HandleAsync(request, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        await _deleteHandler.HandleAsync(id, cancellationToken);
        return NoContent();
    }
}
