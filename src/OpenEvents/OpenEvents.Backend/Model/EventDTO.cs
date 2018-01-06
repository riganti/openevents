using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace OpenEvents.Backend.Model
{
    public class EventDTO : IValidatableObject
    {

        public string Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        public List<EventDateDTO> Dates { get; set; } = new List<EventDateDTO>();

        public List<EventPriceDTO> Prices { get; set; } = new List<EventPriceDTO>();

        public List<EventCancellationPolicyDTO> CancellationPolicies { get; set; } = new List<EventCancellationPolicyDTO>();

        public DateTime RegistrationBeginDate { get; set; }

        public DateTime RegistrationEndDate { get; set; }

        [Range(1, int.MaxValue)]
        public int MaxAttendeeCount { get; set; }



        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!Dates.Any())
            {
                yield return new ValidationResult("The event must specify at least one date!");
            }
            if (Prices.Distinct().Count() > 1)
            {
                yield return new ValidationResult("All event prices must use the same currency!");
            }

            if (RegistrationBeginDate >= RegistrationEndDate)
            {
                yield return new ValidationResult("The date must be greater than registration begin date!", new [] { nameof(RegistrationEndDate) });
            }

            for (var i = 0; i < Dates.Count; i++)
            {
                if (Dates[i].BeginDate < RegistrationEndDate)
                {
                    yield return new ValidationResult("The date must be greater than the registration end date!", new[] { nameof(Dates) + "[" + i + "]." + nameof(EventDateDTO.BeginDate) });
                }
                if (Dates[i].BeginDate >= Dates[i].EndDate)
                {
                    yield return new ValidationResult("The date must be greater than begin date!", new[] { nameof(Dates) + "[" + i + "]." + nameof(EventDateDTO.EndDate) });
                }
            }

            for (var i = 0; i < Prices.Count; i++)
            {
                if (Prices[i].BeginDate < RegistrationBeginDate)
                {
                    yield return new ValidationResult("The date is out of range of the registration dates!", new[] { nameof(Prices) + "[" + i + "]." + nameof(EventPriceDTO.BeginDate) });
                }
                if (Prices[i].EndDate > RegistrationEndDate)
                {
                    yield return new ValidationResult("The date is out of range of the registration dates!", new[] { nameof(Prices) + "[" + i + "]." + nameof(EventPriceDTO.EndDate) });
                }
                if (Prices[i].BeginDate >= Prices[i].EndDate)
                {
                    yield return new ValidationResult("The date must be greater than begin date!", new[] { nameof(Prices) + "[" + i + "]." + nameof(EventPriceDTO.EndDate) });
                }
            }

            for (var i = 0; i < CancellationPolicies.Count; i++)
            {
                if (CancellationPolicies[i].BeginDate < RegistrationBeginDate)
                {
                    yield return new ValidationResult("The date is out of range of the registration dates!", new[] { nameof(CancellationPolicies) + "[" + i + "]." + nameof(EventCancellationPolicyDTO.BeginDate) });
                }
                if (CancellationPolicies[i].EndDate > RegistrationEndDate)
                {
                    yield return new ValidationResult("The date is out of range of the registration dates!", new[] { nameof(CancellationPolicies) + "[" + i + "]." + nameof(EventCancellationPolicyDTO.EndDate) });
                }
            }
        }
    }
}