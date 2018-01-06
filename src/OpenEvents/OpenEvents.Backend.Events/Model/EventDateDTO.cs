using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace OpenEvents.Backend.Events.Model
{
    public class EventDateDTO : IValidatableObject
    {

        public DateTime BeginDate { get; set; }

        public DateTime EndDate { get; set; }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (BeginDate >= EndDate)
            {
                yield return new ValidationResult("The date must be greater than begin date!", new[] { nameof(EndDate) });
            }
        }
    }
}