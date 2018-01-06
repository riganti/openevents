using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenEvents.Backend.Events.Model
{
    public class RegistrationDTO
    {

        public string Id { get; set; }

        public string Sku { get; set; }

        public string SkuDescription { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string OrderId { get; set; }

    }
}