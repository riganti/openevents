using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using OpenEvents.Backend.Common;
using OpenEvents.Backend.Events.Data;
using OpenEvents.Backend.Events.Exceptions;
using OpenEvents.Backend.Events.Model;

namespace OpenEvents.Backend.Events.Controllers
{
    [Route("/api/registrations")]
    public class RegistrationsController : Controller
    {
        private readonly IMongoCollection<Event> eventsCollection;
        private readonly IMongoCollection<RegistrationList> registrationListCollection;

        public RegistrationsController(IMongoCollection<Event> eventsCollection, IMongoCollection<RegistrationList> registrationListCollection)
        {
            this.eventsCollection = eventsCollection;
            this.registrationListCollection = registrationListCollection;
        }

        [HttpGet]
        [Route("{eventId}")]
        public async Task<List<RegistrationDTO>> GetList(string eventId)
        {
            var e = await eventsCollection.FindByIdAsync(eventId);
            if (e == null)
            {
                throw new EventNotFoundException();
            }

            var registrationList = await registrationListCollection.FindByIdAsync(eventId);
            if (registrationList == null)
            {
                return new List<RegistrationDTO>();
            }

            return registrationList.Registrations
                .Select(Mapper.Map<RegistrationDTO>)
                .ToList();
        }

        [HttpGet]
        [Route("{eventId}/count")]
        public async Task<int> GetRegisteredUsersCount(string eventId)
        {
            var e = await eventsCollection.FindByIdAsync(eventId);
            if (e == null)
            {
                throw new EventNotFoundException();
            }

            var registrationList = await registrationListCollection.FindByIdAsync(eventId);
            if (registrationList == null)
            {
                return 0;
            }

            return registrationList.Registrations.Count;
        }

        [HttpPost]
        [Route("{eventId}")]
        public async Task<RegistrationDTO> Create(string eventId, [FromBody] RegistrationDTO registration)
        {
            var e = await eventsCollection.FindByIdAsync(eventId);
            if (e == null)
            {
                throw new EventNotFoundException();
            }
            if (e.Prices.Any())
            {
                throw new UnauthorizedAccessException("Cannot perform this operation for paid events!");
            }

            await registrationListCollection.ChangeOneSafeAsync(eventId, list =>
            {
                if (list.Registrations.Count < e.MaxAttendeeCount)
                {
                    var r = Mapper.Map<Registration>(registration);
                    registration.Id = r.Id = Guid.NewGuid().ToString();

                    list.Registrations.Add(r);
                }
            });

            return registration;
        }

        [HttpDelete]
        [Route("{eventId}/{registrationId}")]
        public async Task Cancel(string eventId, string registrationId)
        {
            var e = await eventsCollection.FindByIdAsync(eventId);
            if (e == null)
            {
                throw new EventNotFoundException();
            }
            if (e.Prices.Any())
            {
                throw new UnauthorizedAccessException("Cannot perform this operation for paid events!");
            }

            await registrationListCollection.ChangeOneSafeAsync(eventId, list =>
            {
                var index = list.Registrations.FindIndex(r => r.Id == registrationId);
                if (index < 0)
                {
                    throw new RegistrationNotFoundException();
                }
                list.Registrations.RemoveAt(index);
            });
        }


        [HttpPost]
        [Route("{eventId}/batch")]
        public async Task<List<RegistrationDTO>> CreateBatch(string eventId, [FromBody] List<RegistrationDTO> registrations)
        {
            var e = await eventsCollection.FindByIdAsync(eventId);
            if (e == null)
            {
                throw new EventNotFoundException();
            }
            
            await registrationListCollection.ChangeOneSafeAsync(eventId, list =>
            {
                foreach (var registration in registrations)
                {
                    var r = Mapper.Map<Registration>(registration);
                    registration.Id = r.Id = Guid.NewGuid().ToString();

                    list.Registrations.Add(r);
                }
            });

            return registrations;
        }

    }
}
