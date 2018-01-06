using System;
using System.Collections.Generic;

namespace OpenEvents.Backend.Data
{
    public class Event : IIdentifiable
    {

        public string Id { get; set; }
        
        public string Title { get; set; }

        public string Description { get; set; }

        public List<EventDate> Dates { get; set; } = new List<EventDate>();

        public List<EventPrice> Prices { get; set; } = new List<EventPrice>();

        public List<EventCancellationPolicy> CancellationPolicies { get; set; } = new List<EventCancellationPolicy>();

        public DateTime RegistrationBeginDate { get; set; }

        public DateTime RegistrationEndDate { get; set; }

        public int MaxAttendeeCount { get; set; }

    }
}