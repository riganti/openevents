using System;
using System.Collections.Generic;
using OpenEvents.Backend.Data;

namespace OpenEvents.Backend.Model
{
    public class EventDTO
    {

        public string Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public List<EventDateDTO> Dates { get; set; } = new List<EventDateDTO>();

        public List<EventPriceDTO> Price { get; set; } = new List<EventPriceDTO>();

        public List<EventCancellationPolicyDTO> CancellationPolicies { get; set; } = new List<EventCancellationPolicyDTO>();

        public DateTime RegistrationBeginDate { get; set; }

        public DateTime RegistrationEndDate { get; set; }

        public int MaxAttendeeCount { get; set; }

    }
}