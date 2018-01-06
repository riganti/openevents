using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using OpenEvents.Backend.Data;
using OpenEvents.Backend.Model;

namespace OpenEvents.Backend.Controllers
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
                throw new HttpResponseException(HttpStatusCode.NotFound);
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

        [HttpPost]
        [Route("{eventId}")]
        public async Task<RegistrationDTO> Create(string eventId, RegistrationDTO registration)
        {
            var e = await eventsCollection.FindByIdAsync(eventId);
            if (e == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            if (e.Prices.Any())
            {
                throw new HttpResponseException(HttpStatusCode.PaymentRequired);
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
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            if (e.Prices.Any())
            {
                throw new HttpResponseException(HttpStatusCode.PaymentRequired);
            }

            await registrationListCollection.ChangeOneSafeAsync(eventId, list =>
            {
                var index = list.Registrations.FindIndex(r => r.Id == registrationId);
                if (index < 0)
                {
                    throw new HttpResponseException(HttpStatusCode.NotFound);
                }
                list.Registrations.RemoveAt(index);
            });
        }

    }
}
