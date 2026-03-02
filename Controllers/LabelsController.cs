using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoWebAPI.Data;
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
    public async Task<ActionResult<IEnumerable<Label>>> GetAll()
    {
        return await _context.Labels.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Label>> GetById(int id)
    {
        var label = await _context.Labels.FindAsync(id);
        if (label == null)
            return NotFound();
        return label;
    }

    [HttpPost]
    public async Task<ActionResult<Label>> Create(Label label)
    {
        _context.Labels.Add(label);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = label.Id }, label);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Label label)
    {
        if (id != label.Id)
            return BadRequest();

        _context.Entry(label).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _context.Labels.AnyAsync(l => l.Id == id))
                return NotFound();
            throw;
        }

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
