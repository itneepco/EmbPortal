using Domain.Common;

namespace Domain.Entities.MeasurementBookAggregate
{
    public class MBItem : AuditableEntity
   {
      public int Id { get; private set; }
      public string Name { get; private set; }
      public int No { get; private set; }
      public int UomId { get; private set; }
      public decimal Rate { get; set; }
      public Uom Uom { get; private set; }
      public float PoQuantity { get; private set; }
      public float CummulativeQuantity { get; private set; } 

      public  MBItem(string name, int no, int UomId, decimal rate,  float poQuantity)
      {
          this.Name = name;
          this.No = no;
          this.UomId = UomId;
          this.Rate = rate;         
          this.PoQuantity = poQuantity;
          this.CummulativeQuantity = 0;
      }

      public void updateCummulativeQuantity(float quantity) 
      {
          this.CummulativeQuantity += quantity;
      }

   }
}