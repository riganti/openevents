using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using OpenEvents.Backend.Common.Exceptions;

namespace OpenEvents.Backend.Common.Filters
{
    public class HttpResponseMessageFilter : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            if (context.Exception is ConflictException ex)
            {
                context.Result = new ContentResult()
                {
                    StatusCode = (int)HttpStatusCode.Conflict,
                    Content = ex.Message,
                    ContentType = "text/plain"
                };
            }
            else if (context.Exception is EntityNotFoundException)
            {
                context.Result = new NotFoundResult();
            }
            else
            {
                base.OnException(context);
            }
        }
    }
}
