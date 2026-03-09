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
    private readonly CreateCategoryHandler _createHandler;
    private readonly UpdateCategoryHandler _updateHandler;
    private readonly DeleteCategoryHandler _deleteHandler;
    private readonly GetAllCategoriesHandler _getAllHandler;
    private readonly GetCategoryByIdHandler _getByIdHandler;

    public CategoriesController(
        CreateCategoryHandler createHandler,
        UpdateCategoryHandler updateHandler,
        DeleteCategoryHandler deleteHandler,
        GetAllCategoriesHandler getAllHandler,
        GetCategoryByIdHandler getByIdHandler)
    {
        _createHandler = createHandler;
        _updateHandler = updateHandler;
        _deleteHandler = deleteHandler;
        _getAllHandler = getAllHandler;
        _getByIdHandler = getByIdHandler;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<GetAllCategoriesResponseDTO>>> GetAll(CancellationToken cancellationToken)
    {
        var result = await _getAllHandler.HandleAsync(cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GetCategoryByIdResponseDTO>> GetById(int id, CancellationToken cancellationToken)
    {
        var result = await _getByIdHandler.HandleAsync(id, cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<int>> Create(CreateCategoryRequestDTO request, CancellationToken cancellationToken)
    {
        var id = await _createHandler.HandleAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateCategoryRequestDTO request, CancellationToken cancellationToken)
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
