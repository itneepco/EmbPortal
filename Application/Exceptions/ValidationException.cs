using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation.Results;

namespace Application.Exceptions
{
   public class ValidationException : Exception
   {
      public ValidationException()
          : base("One or more validation failures have occurred.")
      {
         //Errors = new Dictionary<string, string[]>();
         Errors = new List<string>();
      }

      public ValidationException(List<ValidationFailure> failures)
          : this()
      {
         var propertyNames = failures
             .Select(e => e.PropertyName)
             .Distinct();

         foreach (var propertyName in propertyNames)
         {
            var propertyFailures = failures
                .Where(e => e.PropertyName == propertyName)
                .Select(e => e.ErrorMessage)
                .ToArray();

            //Errors.Add(propertyName, propertyFailures);
            Errors = Errors.Concat(propertyFailures).ToArray();
         }
      }

      //public IDictionary<string, string[]> Errors { get; }
      public IList<string> Errors { get; set; }
   }

}