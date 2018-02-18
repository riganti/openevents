using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotVVM.Framework.Controls;
using DotVVM.Framework.Controls.Bootstrap;
using DotVVM.Framework.Storage;
using DotVVM.Framework.ViewModel;
using OpenEvents.Client;
using OpenEvents.Frontend.Common.Helpers;
using Type = OpenEvents.Client.Type;

namespace OpenEvents.Admin.ViewModels
{
    public class OrderDocumentUploadDialogViewModel : DotvvmViewModelBase
    {
        private readonly IUploadedFileStorage uploadedFileStorage;
        private readonly IOrdersApi ordersApi;


        public bool IsDisplayed { get; set; }

        [FromRoute("id")]
        public string OrderId { get; set; }

        public Type Type { get; set; }

        public UploadedFilesCollection UploadedFiles { get; set; } = new UploadedFilesCollection();

        public List<Type> DocumentTypes => EnumHelper.CreateCollection<Type>();

        public event Func<Task> RefreshRequested;


        public OrderDocumentUploadDialogViewModel(IOrdersApi ordersApi, IUploadedFileStorage uploadedFileStorage)
        {
            this.uploadedFileStorage = uploadedFileStorage;
            this.ordersApi = ordersApi;
        }



        public void Show()
        {
            IsDisplayed = true;
            Type = Type.ProformaInvoice;
            UploadedFiles.Clear();
        }

        public void Hide()
        {
            IsDisplayed = false;
            UploadedFiles.Clear();
        }
        
        public async Task Upload()
        {
            if (UploadedFiles.Files.Any())
            {
                var file = UploadedFiles.Files.First();

                using (var stream = uploadedFileStorage.GetFile(file.FileId))
                {
                    await ordersApi.ApiOrdersDocumentByIdPostAsync(OrderId, Type, new FileParameter(stream, file.FileName));
                }
            }

            Hide();

            await RefreshRequested?.Invoke();
        }

    }
}