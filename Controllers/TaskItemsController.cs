using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoWebAPI.Data;
using ToDoWebAPI.DTOs;
using ToDoWebAPI.Models;

namespace ToDoWebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TaskItemsController : ControllerBase
{
    private readonly AppDbContext _context;

    public TaskItemsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TaskItemResponseDto>>> GetAll()
    {
        var tasks = await _context.TaskItems
            .Include(t => t.Label)
            .Select(t => new TaskItemResponseDto(
                t.Id,
                t.Title,
                t.Description,
                t.DueDate,
                t.LabelId,
                t.Label != null ? t.Label.Name : null
            ))
            .ToListAsync();

        return tasks;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TaskItemResponseDto>> GetById(int id)
    {
        var task = await _context.TaskItems
            .Include(t => t.Label)
            .Where(t => t.Id == id)
            .Select(t => new TaskItemResponseDto(
                t.Id,
                t.Title,
                t.Description,
                t.DueDate,
                t.LabelId,
                t.Label != null ? t.Label.Name : null
            ))
            .FirstOrDefaultAsync();

        if (task == null)
            return NotFound();

        return task;
    }

    [HttpPost]
    public async Task<ActionResult<TaskItemResponseDto>> Create(TaskItemCreateDto dto)
    {
        var taskItem = new TaskItem
        {
            Title = dto.Title,
            Description = dto.Description,
            DueDate = dto.DueDate,
            LabelId = dto.LabelId
        };

        _context.TaskItems.Add(taskItem);
        await _context.SaveChangesAsync();

        if (taskItem.LabelId.HasValue)
        {
            await _context.Entry(taskItem).Reference(t => t.Label).LoadAsync();
        }

        var response = new TaskItemResponseDto(
            taskItem.Id,
            taskItem.Title,
            taskItem.Description,
            taskItem.DueDate,
            taskItem.LabelId,
            taskItem.Label?.Name
        );

        return CreatedAtAction(nameof(GetById), new { id = taskItem.Id }, response);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, TaskItemUpdateDto dto)
    {
        if (id != dto.Id)
            return BadRequest();

        var taskItem = await _context.TaskItems.FindAsync(id);
        if (taskItem == null)
            return NotFound();

        taskItem.Title = dto.Title;
        taskItem.Description = dto.Description;
        taskItem.DueDate = dto.DueDate;
        taskItem.LabelId = dto.LabelId;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var task = await _context.TaskItems.FindAsync(id);
        if (task == null)
            return NotFound();

        _context.TaskItems.Remove(task);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
