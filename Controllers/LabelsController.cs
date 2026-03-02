using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoWebAPI.Data;
using ToDoWebAPI.DTOs;
using ToDoWebAPI.Models;

namespace ToDoWebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LabelsController : ControllerBase
{
    private readonly AppDbContext _context;

    public LabelsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<LabelResponseDto>>> GetAll()
    {
        var labels = await _context.Labels
            .Select(l => new LabelResponseDto(l.Id, l.Name))
            .ToListAsync();

        return labels;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<LabelResponseDto>> GetById(int id)
    {
        var label = await _context.Labels
            .Where(l => l.Id == id)
            .Select(l => new LabelResponseDto(l.Id, l.Name))
            .FirstOrDefaultAsync();

        if (label == null)
            return NotFound();

        return label;
    }

    [HttpPost]
    public async Task<ActionResult<LabelResponseDto>> Create(LabelCreateDto dto)
    {
        var label = new Label
        {
            Name = dto.Name
        };

        _context.Labels.Add(label);
        await _context.SaveChangesAsync();

        var response = new LabelResponseDto(label.Id, label.Name);

        return CreatedAtAction(nameof(GetById), new { id = label.Id }, response);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, LabelUpdateDto dto)
    {
        if (id != dto.Id)
            return BadRequest();

        var label = await _context.Labels.FindAsync(id);
        if (label == null)
            return NotFound();

        label.Name = dto.Name;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var label = await _context.Labels.FindAsync(id);
        if (label == null)
            return NotFound();

        _context.Labels.Remove(label);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
