using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace OpenEvents.Backend.Common.Filters
{
    public class EmptyResultWithStatusCode : IActionResult
    {

        public int StatusCode { get; set; }

        public Task ExecuteResultAsync(ActionContext context)
        {
            context.HttpContext.Response.StatusCode = StatusCode;
            return Task.CompletedTask;
        }
    }
}