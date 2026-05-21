namespace ExpenseTracker.Models;

public class ExpenseItem
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool IsActive { get; set; } = true;

    public int CategoryId { get; set; }
    public Category Category { get; set; }

    public List<Transaction> Transactions { get; set; }
}