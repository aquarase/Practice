namespace ExpenseTracker.Models;

public class Transaction
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public decimal Amount { get; set; }
    public string? Comment { get; set; }

    public int? ExpenseItemId { get; set; }
    public ExpenseItem? ExpenseItem { get; set; }
}