using RentalTrack.Utility.Enums;

namespace RentalTrack.Data.Entities;

public class Expense
{
    public long Id { get; set; }
    public long PropertyId { get; set; }
    public ExpenseCategory Category { get; set; }
    public decimal Amount { get; set; }
    public DateTime ExpenseDate { get; set; }
    public string? Notes { get; set; }
    public bool IsRecurring { get; set; }
    public RecurrencePeriod? RecurrencePeriod { get; set; }
    public DateTime? NextOccurrenceDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }
}
