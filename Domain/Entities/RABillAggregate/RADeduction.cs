namespace Domain.Entities.RABillAggregate
{
    public class RADeduction
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public int RABillId { get; set; }       

        public RADeduction()
        {
        }

        public RADeduction(string desc, decimal amt)
        {
            Description = desc;
            Amount = amt;
        }
    }
}
