﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenEvents.Backend.Model
{
    public class EventPriceDTO
    {

        public DateTime BeginDate { get; set; }

        public DateTime EndDate { get; set; }

        public decimal Price { get; set; }

        public string CurrencyCode { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }
    }
}