using System;
using System.Collections.Generic;
using System.Linq;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace OpenEvents.Backend.Common.Filters
{
    public class FileResponseOperationFilter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            operation.Produces = new[]
            {
                "application/octet-stream"
            };

            operation.Responses["200"].Schema = new Schema
            {
                Type = "file" 
            };
        }
    }
}