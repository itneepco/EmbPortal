using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Exceptions
{
   public class UnauthorizedUserException : Exception
   {
      public UnauthorizedUserException() : base()
      {
      }

      public UnauthorizedUserException(string message) : base(message)
      {
      }

   }
}