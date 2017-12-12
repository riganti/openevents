using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using NSwag;
using NSwag.CodeGeneration.CSharp;

namespace OpenEvents.Backend.Client.Generator
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // TODO: check the commandline arguments
            try
            {
                var url = args[0];
                var targetDir = args[1];

                var document = await GetSwaggerDocument(url);
                var code = GenerateCode(document);
                WriteFile(targetDir, code);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Environment.Exit(1);
            }
        }

        private static async Task<SwaggerDocument> GetSwaggerDocument(string url)
        {
            var webClient = new WebClient();
            var json = await webClient.DownloadStringTaskAsync(url);
            var document = await SwaggerDocument.FromJsonAsync(json);
            return document;
        }

        private static string GenerateCode(SwaggerDocument document)
        {
            var settings = new SwaggerToCSharpClientGeneratorSettings
            {
                ClassName = "ApiClient",
                CSharpGeneratorSettings =
                {
                    Namespace = "OpenEvents.Backend.Client"
                }
            };

            var generator = new SwaggerToCSharpClientGenerator(document, settings);
            var code = generator.GenerateFile();
            return code;
        }

        private static void WriteFile(string targetDir, string code)
        {
            var path = Path.Combine(targetDir, "ApiClient.cs");
            File.WriteAllText(path, code);
        }
    }
}
