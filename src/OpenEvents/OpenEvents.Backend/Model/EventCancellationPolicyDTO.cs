using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace OpenEvents.Backend.Model
{
    public class EventCancellationPolicyDTO : IValidatableObject
    {
        public DateTime BeginDate { get; set; }

        public DateTime EndDate { get; set; }

        [Range(0, 100)]
        public decimal CancellationFeePercent { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (BeginDate >= EndDate)
            {
                yield return new ValidationResult("The date must be greater than begin date!", new[] { nameof(EndDate) });
            }
        }
    }
}