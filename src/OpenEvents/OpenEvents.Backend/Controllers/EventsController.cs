using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using OpenEvents.Backend.Data;
using OpenEvents.Backend.Filters;
using OpenEvents.Backend.Model;

namespace OpenEvents.Backend.Controllers
{
    [Route("/api/events")]
    public class EventsController : Controller
    {

        // TODO: return correct HTTP codes

        private readonly IMongoCollection<Event> eventsCollection;

        public EventsController(IMongoCollection<Event> eventsCollection)
        {
            this.eventsCollection = eventsCollection;
        }

        [HttpGet]
        public List<EventDTO> GetList()
        {
            // TODO: add paging and filtering
            
            return eventsCollection.AsQueryable()
                .Select(Mapper.Map<EventDTO>)
                .ToList();
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<EventDTO> Get(string id)
        {
            var e = await eventsCollection.FindByIdAsync(id);
            if (e == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            return Mapper.Map<EventDTO>(e);
        }


        [HttpPost]
        [ModelValidationFilter]
        [ProducesResponseType(typeof(EventDTO), (int)HttpStatusCode.Created)]
        public async Task<IActionResult> Create([FromBody] EventDTO eventData)
        {
            var e = Mapper.Map<Event>(eventData);
            e.Id = Guid.NewGuid().ToString();

            await eventsCollection.InsertOneAsync(e);

            return CreatedAtAction(nameof(Get), new { id = e.Id });
        }


        [HttpPut]
        [Route("{id}")]
        [ModelValidationFilter]
        public async Task Update(string id, [FromBody] EventDTO eventData)
        {
            // TODO: use etag

            var e = await eventsCollection.FindByIdAsync(id);
            if (e == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            Mapper.Map(eventData, e);
            await eventsCollection.ReplaceOneAsync(ev => ev.Id == id, e);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task Delete(string id)
        {
            // TODO: soft deletes

            await eventsCollection.DeleteOneAsync(ev => ev.Id == id);
        }


    }
}
