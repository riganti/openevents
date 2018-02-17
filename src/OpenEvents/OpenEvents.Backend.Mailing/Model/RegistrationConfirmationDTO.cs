using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OpenEvents.Client;

namespace OpenEvents.Backend.Mailing.Model
{
    public class RegistrationConfirmationDTO
    {

        public EventDTO Event { get; set; }

        public List<RegistrationDTO> Registrations { get; set; }

    }
}
