using System;
using System.Collections.Generic;
using System.Linq;
using OpenEvents.Backend.Common;

namespace OpenEvents.Backend.Model
{
    public class OrderDocumentDTO
    {
        public string Id { get; set; }

        public string Url { get; set; }

        public OrderDocumentType Type { get; set; }

        public DateTime CreatedDate { get; set; }

    }
}