using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenEvents.Backend.Data
{
    public class EventCancellationPolicy
    {
        public DateTime BeginDate { get; set; }

        public DateTime EndDate { get; set; }

        public decimal CancellationFeePercent { get; set; }

    }
}