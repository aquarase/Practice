using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ExpenseTracker.Data;
using ExpenseTracker.DTOs;
using ExpenseTracker.Models;

namespace ExpenseTracker.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ExpenseItemsController : ControllerBase
{
    private readonly AppDbContext _context;

    public ExpenseItemsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<List<ExpenseItemDto>>> GetAll()
    {
        return await _context.ExpenseItems
            .Include(e => e.Category)
            .Select(e => new ExpenseItemDto
            {
                Id = e.Id,
                Name = e.Name,
                CategoryId = e.CategoryId,
                CategoryName = e.Category.Name,
                IsActive = e.IsActive
            })
            .ToListAsync();
    }

    [HttpPost]
    public async Task<ActionResult<ExpenseItemDto>> Create(CreateExpenseItemDto dto)
    {
        var item = new ExpenseItem
        {
            Name = dto.Name,
            CategoryId = dto.CategoryId,
            IsActive = dto.IsActive
        };

        _context.ExpenseItems.Add(item);
        await _context.SaveChangesAsync();

        await _context.Entry(item).Reference(i => i.Category).LoadAsync();

        return CreatedAtAction(nameof(GetById), new { id = item.Id },
            new ExpenseItemDto
            {
                Id = item.Id,
                Name = item.Name,
                CategoryId = item.CategoryId,
                CategoryName = item.Category?.Name,  
                IsActive = item.IsActive
            });
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ExpenseItemDto>> GetById(int id)
    {
        var item = await _context.ExpenseItems
            .Include(e => e.Category)
            .FirstOrDefaultAsync(e => e.Id == id);

        if (item == null) return NotFound();

        return new ExpenseItemDto
        {
            Id = item.Id,
            Name = item.Name,
            CategoryId = item.CategoryId,
            CategoryName = item.Category?.Name,
            IsActive = item.IsActive
        };
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, CreateExpenseItemDto dto)
    {
        var item = await _context.ExpenseItems.FindAsync(id);
        if (item == null) return NotFound();

        item.Name = dto.Name;
        item.CategoryId = dto.CategoryId;
        item.IsActive = dto.IsActive;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var item = await _context.ExpenseItems.FindAsync(id);
        if (item == null) return NotFound();

        _context.ExpenseItems.Remove(item);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}