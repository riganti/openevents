using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotVVM.Framework.Hosting;
using DotVVM.Framework.ViewModel.Validation;
using Newtonsoft.Json;
using OpenEvents.Client;

namespace OpenEvents.Admin.Helpers
{
    public static class ValidationHelper
    {

        public static async Task Call(IDotvvmRequestContext context, Func<Task> apiCall)
        {
            try
            {
                await apiCall();
            }
            catch (SwaggerException ex) when (ex.StatusCode == "400")
            {
                HandleValidation(context, ex);
            }
        }

        public static async Task<T> Call<T>(IDotvvmRequestContext context, Func<Task<T>> apiCall)
        {
            try
            {
                return await apiCall();
            }
            catch (SwaggerException ex) when (ex.StatusCode == "400")
            {
                HandleValidation(context, ex);
                return default(T);
            }
        }

        private static void HandleValidation(IDotvvmRequestContext context, SwaggerException ex)
        {
            var invalidProperties = JsonConvert.DeserializeObject<Dictionary<string, string[]>>(ex.Response);

            foreach (var property in invalidProperties)
            {
                foreach (var error in property.Value)
                {
                    context.ModelState.Errors.Add(new ViewModelValidationError() { PropertyPath = ConvertPropertyName(property.Key), ErrorMessage = error });
                }
            }

            context.FailOnInvalidModelState();
        }

        private static string ConvertPropertyName(string property)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < property.Length; i++)
            {
                if (property[i] == '.')
                {
                    sb.Append("().");
                }
                else if (property[i] == '[')
                {
                    sb.Append("()[");
                }
                else
                {
                    sb.Append(property[i]);
                }
            }
            return sb.ToString();
        }
    }
}
