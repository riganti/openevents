using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace OpenEvents.Backend.Common.Filters
{
    public class UploadedFileOperationFilter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            var hasFile = false;

            // find all parameters of type IFormFile
            foreach (var paramDescription in context.ApiDescription.ParameterDescriptions.Where(p => p.Type == typeof(IFormFile)))
            {
                var operationParam = operation.Parameters.Single(p => p.Name == paramDescription.Name);

                // remove it
                var index = operation.Parameters.IndexOf(operationParam);
                operation.Parameters.RemoveAt(index);

                // replace with a new one
                operation.Parameters.Insert(index, new NonBodyParameter()
                {
                    Name = operationParam.Name,
                    In = "formData",
                    Description = "Upload File",
                    Required = true,
                    Type = "file"
                });

                hasFile = true;
            }

            // add multipart/form-data input format
            if (hasFile)
            {
                operation.Consumes.Add("multipart/form-data");
            }
        }
    }
}
