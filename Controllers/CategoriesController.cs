using MediatR;
using Microsoft.AspNetCore.Mvc;
using ToDoWebAPI.Features.Commands.Categories.CreateCategory;
using ToDoWebAPI.Features.Commands.Categories.DeleteCategory;
using ToDoWebAPI.Features.Commands.Categories.UpdateCategory;
using ToDoWebAPI.Features.Queries.Categories.GetAllCategories;
using ToDoWebAPI.Features.Queries.Categories.GetCategoryById;

namespace ToDoWebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly IMediator _mediator;

    public CategoriesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<GetAllCategoriesResponseDTO>>> GetAll(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAllCategoriesQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GetCategoryByIdResponseDTO>> GetById(int id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetCategoryByIdQuery(id), cancellationToken);

        if (result == null)
            return NotFound();

        return result;
    }

    [HttpPost]
    public async Task<ActionResult<CreateCategoryResponseDTO>> Create(CreateCategoryRequestDTO request, CancellationToken cancellationToken)
    {
        var command = new CreateCategoryCommand(request.Name, request.Description, request.Color);
        var result = await _mediator.Send(command, cancellationToken);

        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateCategoryRequestDTO request, CancellationToken cancellationToken)
    {
        if (id != request.Id)
            return BadRequest();

        var command = new UpdateCategoryCommand(request.Id, request.Name, request.Description, request.Color);
        var success = await _mediator.Send(command, cancellationToken);

        if (!success)
            return NotFound();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var command = new DeleteCategoryCommand(id);
        var success = await _mediator.Send(command, cancellationToken);

        if (!success)
            return NotFound();

        return NoContent();
    }
}
