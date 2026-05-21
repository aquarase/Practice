using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ExpenseTracker.Data;
using ExpenseTracker.DTOs;
using ExpenseTracker.Models;

namespace ExpenseTracker.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoriesController : ControllerBase
{
    private readonly AppDbContext _context;

    public CategoriesController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<List<CategoryDto>>> GetAll()
    {
        return await _context.Categories
            .Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                MonthlyBudget = c.MonthlyBudget,
                IsActive = c.IsActive
            })
            .ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CategoryDto>> GetById(int id)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category == null) return NotFound();

        return new CategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            MonthlyBudget = category.MonthlyBudget,
            IsActive = category.IsActive
        };
    }

    [HttpPost]
    public async Task<ActionResult<CategoryDto>> Create(CreateCategoryDto dto)
    {
        var category = new Category
        {
            Name = dto.Name,
            MonthlyBudget = dto.MonthlyBudget,
            IsActive = dto.IsActive
        };

        _context.Categories.Add(category);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = category.Id },
            new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                MonthlyBudget = category.MonthlyBudget,
                IsActive = category.IsActive
            });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateCategoryDto dto)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category == null) return NotFound();

        category.Name = dto.Name;
        category.MonthlyBudget = dto.MonthlyBudget;
        category.IsActive = dto.IsActive;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category == null) return NotFound();

        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}