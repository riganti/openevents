using System;
using System.Collections.Generic;
using System.Linq;
using OpenEvents.Backend.Common.Data;

namespace OpenEvents.Backend.Events.Data
{
    public class RegistrationList : IIdentifiable, IVersionedEntity
    {

        public string Id { get; set; }

        public string ETag { get; set; }

        public List<Registration> Registrations { get; set; } = new List<Registration>();


    }
}
