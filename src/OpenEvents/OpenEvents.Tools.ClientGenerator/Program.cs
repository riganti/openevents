using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using NSwag;
using NSwag.CodeGeneration.CSharp;

namespace OpenEvents.Tools.ClientGenerator
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();

            var hasErrors = false;
            var clients = config.GetSection("clients").Get<ClientConfiguration[]>();
            foreach (var client in clients)
            {
                try
                {
                    Console.WriteLine($"Generating client {client.ClassName}...");

                    var document = await GetSwaggerDocument(client.SwaggerUrl);
                    var code = GenerateCode(document, client.ClassName);
                    WriteFile(client.OutputPath, client.ClassName, code);

                    Console.WriteLine($"Success");
                    Console.WriteLine();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    Console.WriteLine();
                    hasErrors = true;
                }
            }

            if (hasErrors)
            {
                if (Debugger.IsAttached)
                {
                    Console.WriteLine("Press any key to exit...");
                    Console.ReadKey();
                }
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

        private static string GenerateCode(SwaggerDocument document, string className)
        {
            var settings = new SwaggerToCSharpClientGeneratorSettings
            {
                ClassName = className,
                CSharpGeneratorSettings =
                {
                    Namespace = "OpenEvents.Client"
                },
                GenerateExceptionClasses = false,
                GenerateClientInterfaces = true
            };

            var generator = new SwaggerToCSharpClientGenerator(document, settings);
            var code = generator.GenerateFile();
            return code;
        }

        private static void WriteFile(string targetDir, string className, string code)
        {
            var path = Path.Combine(targetDir, className + ".cs");
            File.WriteAllText(path, code);
        }
    }
}
