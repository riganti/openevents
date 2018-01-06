using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace OpenEvents.Backend.Filters
{
    public class HttpResponseMessageFilter : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            if (context.Exception is HttpResponseException ex)
            {
                if (ex.Content != null)
                {
                    context.Result = new ObjectResult(ex.Content)
                    {
                        StatusCode = (int) ex.StatusCode
                    };
                }
                else
                {
                    context.Result = new EmptyResultWithStatusCode()
                    {
                        StatusCode = (int)ex.StatusCode
                    };
                }
            }
            else
            {
                base.OnException(context);
            }
        }
    }
}
