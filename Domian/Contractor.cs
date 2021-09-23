using Domian.Common;

namespace Domian
{
   public class Contractor : AuditableEntity
   {
      public int Id { get; private set; }
      public string Name { get; private set; }

      private Contractor() { }

      public Contractor(string name)
      {
         Name = name;
      }

      public void SetName(string name) 
      {
         Name = name;
      }

   }
}
