namespace ExpenseTracker.Models;

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal MonthlyBudget { get; set; }
    public bool IsActive { get; set; } = true;

    public List<ExpenseItem> Items { get; set; }
}