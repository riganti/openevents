using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using OpenEvents.Backend.Common;
using OpenEvents.Backend.Common.Configuration;

namespace OpenEvents.Backend.Orders.Services
{
    public class DocumentStorageService : IAppInitializerTask
    {

        private readonly CloudBlobContainer invoicesContainer;
        

        public DocumentStorageService(DocumentStorageConfiguration configuration)
        {
            var account = CloudStorageAccount.Parse(configuration.ConnectionString);
            var client = account.CreateCloudBlobClient();

            invoicesContainer = client.GetContainerReference(configuration.Environment + "-invoices");
        }

        public async Task Initialize()
        {
            await invoicesContainer.CreateIfNotExistsAsync();
        }


        public async Task<string> UploadInvoice(Stream stream)
        {
            var url = Guid.NewGuid() + ".pdf";

            var blob = invoicesContainer.GetBlockBlobReference(url);
            await blob.UploadFromStreamAsync(stream);

            return url;
        }

        public async Task<Stream> DownloadInvoice(string url)
        {
            var blob = invoicesContainer.GetBlockBlobReference(url);
            return await blob.OpenReadAsync();
        }
    }
}
