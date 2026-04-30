namespace RentalTrack.Data.Entities;

public class RentalPayment
{
    public long Id { get; set; }
    public long PropertyId { get; set; }
    public short Year { get; set; }
    public byte  Month { get; set; }
    public decimal ExpectedAmount { get; set; }
    public decimal PaidAmount { get; set; }
    public bool IsPaid { get; set; }
    public DateTime? DatePaid { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
