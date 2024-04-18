namespace Domain.Entities.RAAggregate;

public class Deduction
{
    public int Id { get; set; }
    public string Description { get; set; }
    public decimal Amount { get; set; }
    //public int RABillId { get; set; }
}
