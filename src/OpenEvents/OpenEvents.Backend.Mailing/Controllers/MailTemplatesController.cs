using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OpenEvents.Backend.Mailing.Facades;
using OpenEvents.Backend.Mailing.Model;

namespace OpenMailTemplates.Backend.Mailing.Controllers
{
    [Route("/api/mailtemplates")]
    public class MailTemplatesController : Controller
    {
        private readonly MailTemplatesFacade mailTemplatesFacade;

        public MailTemplatesController(MailTemplatesFacade mailTemplatesFacade)
        {
            this.mailTemplatesFacade = mailTemplatesFacade;
        }

        [HttpGet]
        public Task<List<MailTemplateDTO>> GetList([FromQuery] MailTemplateFilterDTO filter)
        {
            return mailTemplatesFacade.GetAll(filter);
        }
        
        [HttpGet]
        [Route("{id}")]
        public Task<MailTemplateDTO> Get(string id)
        {
            return mailTemplatesFacade.GetById(id);
        }

        [HttpPost]
        [ProducesResponseType(typeof(MailTemplateDTO), (int)HttpStatusCode.Created)]
        public async Task<IActionResult> Create([FromBody] MailTemplateDTO mailTemplatesData)
        {
            await mailTemplatesFacade.Insert(mailTemplatesData);

            return CreatedAtAction(nameof(Get), new { id = mailTemplatesData.Id }, mailTemplatesData);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task Update(string id, [FromBody] MailTemplateDTO mailTemplatesData)
        {
            await mailTemplatesFacade.Update(id, mailTemplatesData);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task Delete(string id)
        {
            await mailTemplatesFacade.Delete(id);
        }

    }
}
