using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace OpenEvents.Backend.Model
{
    public class EventPriceDTO : IValidatableObject
    {

        public DateTime BeginDate { get; set; }

        public DateTime EndDate { get; set; }

        [Range(0, int.MaxValue)]
        public decimal Price { get; set; }

        [Required]
        [RegularExpression("^[A-Z]{3}$")]
        public string CurrencyCode { get; set; }

        [Required]
        public string Title { get; set; }

        public string Description { get; set; }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (BeginDate >= EndDate)
            {
                yield return new ValidationResult("The date must be greater than begin date!", new[] { nameof(EndDate) });
            }
        }
    }
}