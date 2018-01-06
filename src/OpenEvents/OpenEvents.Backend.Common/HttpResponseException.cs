using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace OpenEvents.Backend.Common
{
    public class HttpResponseException : Exception
    {
        public HttpStatusCode StatusCode { get; }

        public object Content { get; }

        public HttpResponseException(HttpStatusCode statusCode) : base($"HTTP {(int)statusCode} {statusCode}")
        {
            StatusCode = statusCode;
        }

        public HttpResponseException(HttpStatusCode statusCode, object content) : this(statusCode)
        {
            Content = content;
        }

    }
}
