using System;

namespace Domain.Exceptions
{
    public class EntityException : Exception
   {
      public EntityException()
         : base()
      {
      }

      public EntityException(string message)
          : base(message)
      {
      }

      public EntityException(string entityName, string message)
          : base($"Entity \"{entityName}\" error: \"{message}\"")
      {
      }
   }
}