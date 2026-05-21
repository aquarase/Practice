using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ExpenseTracker.Data;
using ExpenseTracker.DTOs;
using ExpenseTracker.Models;

namespace ExpenseTracker.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TransactionsController : ControllerBase
{
    private readonly AppDbContext _context;

    public TransactionsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<ActionResult<TransactionDto>> Create(CreateTransactionDto dto)
    {
        // Проверка активности статьи
        if (dto.ExpenseItemId.HasValue)
        {
            var item = await _context.ExpenseItems.FindAsync(dto.ExpenseItemId.Value);
            if (item == null || !item.IsActive)
                return BadRequest("Статья расхода неактивна или не существует");
        }

        // Проверка лимита 1 млн в день
        var date = dto.Date.Date;
        var dailySum = await _context.Transactions
            .Where(t => t.Date == date)
            .SumAsync(t => (decimal?)t.Amount) ?? 0;

        if (dailySum + dto.Amount > 1_000_000)
            return BadRequest("Превышен дневной лимит в 1 000 000 руб.");

        var transaction = new Transaction
        {
            Date = date,
            Amount = dto.Amount,
            Comment = dto.Comment,
            ExpenseItemId = dto.ExpenseItemId
        };

        _context.Transactions.Add(transaction);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = transaction.Id }, MapToDto(transaction));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TransactionDto>> GetById(int id)
    {
        var transaction = await _context.Transactions
            .Include(t => t.ExpenseItem)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (transaction == null) return NotFound();
        return MapToDto(transaction);
    }

    [HttpGet]
    public async Task<ActionResult<List<TransactionDto>>> GetAll(
        [FromQuery] DateTime? date,
        [FromQuery] int? month,
        [FromQuery] int? year)
    {
        var query = _context.Transactions
            .Include(t => t.ExpenseItem)
            .AsQueryable();

        if (date.HasValue)
            query = query.Where(t => t.Date == date.Value.Date);
        else if (month.HasValue && year.HasValue)
            query = query.Where(t => t.Date.Month == month && t.Date.Year == year);

        return await query
            .OrderByDescending(t => t.Date)
            .Select(t => MapToDto(t))
            .ToListAsync();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, CreateTransactionDto dto)
    {
        var transaction = await _context.Transactions.FindAsync(id);
        if (transaction == null) return NotFound();

        // Если статья стала неактивной, запрещаем менять
        if (transaction.ExpenseItemId.HasValue)
        {
            var item = await _context.ExpenseItems.FindAsync(transaction.ExpenseItemId.Value);
            if (item != null && !item.IsActive && dto.ExpenseItemId != transaction.ExpenseItemId)
                return BadRequest("Нельзя изменить статью расхода — она неактивна");
        }

        // Проверка лимита
        if (dto.Amount != transaction.Amount)
        {
            var dailySum = await _context.Transactions
                .Where(t => t.Date == transaction.Date && t.Id != id)
                .SumAsync(t => (decimal?)t.Amount) ?? 0;

            if (dailySum + dto.Amount > 1_000_000)
                return BadRequest("Превышен дневной лимит в 1 000 000 руб.");
        }

        transaction.Amount = dto.Amount;
        transaction.Comment = dto.Comment;
        transaction.ExpenseItemId = dto.ExpenseItemId;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var transaction = await _context.Transactions.FindAsync(id);
        if (transaction == null) return NotFound();

        _context.Transactions.Remove(transaction);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpGet("summary/{date}")]
    public async Task<ActionResult<DaySummaryDto>> GetDaySummary(DateTime date)
    {
        var total = await _context.Transactions
            .Where(t => t.Date == date.Date)
            .SumAsync(t => (decimal?)t.Amount) ?? 0;

        string color = total switch
        {
            < 500 => "green",
            <= 2000 => "yellow",
            _ => "red"
        };

        return new DaySummaryDto
        {
            Date = date.Date,
            TotalAmount = total,
            StickerColor = color
        };
    }

    private TransactionDto MapToDto(Transaction t) => new()
    {
        Id = t.Id,
        Date = t.Date,
        Amount = t.Amount,
        Comment = t.Comment,
        ExpenseItemId = t.ExpenseItemId,
        ExpenseItemName = t.ExpenseItem?.Name
    };
}