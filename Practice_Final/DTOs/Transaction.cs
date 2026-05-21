namespace ExpenseTracker.DTOs;

public class TransactionDto
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public decimal Amount { get; set; }
    public string? Comment { get; set; }
    public int? ExpenseItemId { get; set; }
    public string? ExpenseItemName { get; set; }
}

public class CreateTransactionDto
{
    public DateTime Date { get; set; }
    public decimal Amount { get; set; }
    public string? Comment { get; set; }
    public int? ExpenseItemId { get; set; }
}

public class DaySummaryDto
{
    public DateTime Date { get; set; }
    public decimal TotalAmount { get; set; }
    public string StickerColor { get; set; }
}