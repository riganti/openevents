using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenEvents.Backend.Data
{
    public class RegistrationList : IIdentifiable, IVersionedEntity
    {

        public string Id { get; set; }

        public string ETag { get; set; }

        public List<Registration> Registrations { get; set; } = new List<Registration>();


    }
}
