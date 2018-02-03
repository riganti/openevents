using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using OpenEvents.Backend.Common;
using OpenEvents.Backend.Common.Filters;
using OpenEvents.Backend.Events.Facades;
using OpenEvents.Backend.Events.Model;

namespace OpenEvents.Backend.Events.Controllers
{
    [Route("/api/events")]
    public class EventsController : Controller
    {
        private readonly EventsFacade eventsFacade;

        public EventsController(EventsFacade eventsFacade)
        {
            this.eventsFacade = eventsFacade;
        }

        [HttpGet]
        public Task<List<EventDTO>> GetList()
        {
            return eventsFacade.GetAll();
        }

        [HttpGet]
        [Route("basic")]
        public Task<List<EventBasicDTO>> GetBasicList()
        {
            return eventsFacade.GetAllBasic();
        }

        [HttpGet]
        [Route("{id}")]
        public Task<EventDTO> Get(string id)
        {
            return eventsFacade.GetById(id);
        }

        [HttpPost]
        [ProducesResponseType(typeof(EventDTO), (int)HttpStatusCode.Created)]
        public async Task<IActionResult> Create([FromBody] EventDTO eventData)
        {
            await eventsFacade.Insert(eventData);

            return CreatedAtAction(nameof(Get), new { id = eventData.Id }, eventData);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task Update(string id, [FromBody] EventDTO eventData)
        {
            // TODO: use etag
            await eventsFacade.Update(id, eventData);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task Delete(string id)
        {
            // TODO: soft deletes
            await eventsFacade.Delete(id);
        }

    }
}
