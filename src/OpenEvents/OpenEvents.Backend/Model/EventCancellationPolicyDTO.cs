using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenEvents.Backend.Model
{
    public class EventCancellationPolicyDTO
    {
        public DateTime BeginDate { get; set; }

        public DateTime EndDate { get; set; }

        public decimal CancellationFeePercent { get; set; }

    }
}