using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using OpenEvents.Backend.Data;
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
        public async Task<List<EventDTO>> GetList()
        {
            // TODO: add paging and filtering

            var events = await eventsCollection.FindAsync(FilterDefinition<Event>.Empty);

            return events.ToList()
                .Select(Mapper.Map<EventDTO>)
                .ToList();
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<EventDTO> Get(string id)
        {
            // TODO: add paging and filtering

            var e = (await eventsCollection.FindAsync(ev => ev.Id == id)).First();
            return Mapper.Map<EventDTO>(e);
        }


        [HttpPost]
        public async Task Create([FromBody] EventDTO eventData)
        {
            var e = Mapper.Map<Event>(eventData);
            e.Id = Guid.NewGuid().ToString();

            await eventsCollection.InsertOneAsync(e);
        }


        [HttpPut]
        [Route("{id}")]
        public async Task Update(string id, [FromBody] EventDTO eventData)
        {
            // TODO: populate existing entity instead of replacing it
            // TODO: use etag

            var e = (await eventsCollection.FindAsync(ev => ev.Id == id)).First();
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
