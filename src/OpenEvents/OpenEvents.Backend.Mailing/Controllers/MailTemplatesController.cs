using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OpenEvents.Backend.Mailing.Facades;
using OpenEvents.Backend.Mailing.Model;
using OpenEvents.Backend.Mailing.Services;

namespace OpenMailTemplates.Backend.Mailing.Controllers
{
    [Route("/api/mailtemplates")]
    public class MailTemplatesController : Controller
    {
        private readonly MailTemplatesFacade mailTemplatesFacade;
        private readonly TemplateProcessor templateProcessor;

        public MailTemplatesController(MailTemplatesFacade mailTemplatesFacade, TemplateProcessor templateProcessor)
        {
            this.mailTemplatesFacade = mailTemplatesFacade;
            this.templateProcessor = templateProcessor;
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
        public async Task<IActionResult> Create([FromBody] MailTemplateDTO mailTemplateData)
        {
            await mailTemplatesFacade.Insert(mailTemplateData);

            return CreatedAtAction(nameof(Get), new { id = mailTemplateData.Id }, mailTemplateData);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task Update(string id, [FromBody] MailTemplateDTO mailTemplateData)
        {
            await mailTemplatesFacade.Update(id, mailTemplateData);
        }

        [HttpPost]
        [Route("test")]
        public async Task<string> Test([FromBody] MailTemplateDTO mailTemplateData)
        {
            return await templateProcessor.TestTemplate(mailTemplateData);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task Delete(string id)
        {
            await mailTemplatesFacade.Delete(id);
        }

    }
}
