using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotVVM.Framework.Storage;
using DotVVM.Framework.ViewModel;
using OpenEvents.Client;
using OpenEvents.Frontend.Common.Helpers;

namespace OpenEvents.Admin.ViewModels
{
    public class OrderDetailViewModel : MasterPageViewModel
    {
        private readonly IOrdersApi client;

        public OrderDetailViewModel(IOrdersApi client, IUploadedFileStorage uploadedFileStorage)
        {
            this.client = client;

            UploadDialog = new OrderDocumentUploadDialogViewModel(client, uploadedFileStorage);
        }

        public override string CurrentSection => "Orders";

        public OrderDTO Item { get; set; }

        [FromRoute("id")]
        public string ItemId { get; private set; }

        public OrderEditMode EditMode { get; set; } = OrderEditMode.ReadOnly;

        [Bind(Direction.ServerToClientFirstRequest)]
        public List<CountryDTO> Countries { get; set; }

        public OrderDocumentUploadDialogViewModel UploadDialog { get; set; }


        public override Task Init()
        {
            if (!Context.IsPostBack)
            {
                Countries = new List<CountryDTO>()
                {
                    new CountryDTO() { Name = "Czech Republic", Code = "CZ" },
                    new CountryDTO() { Name = "Slovakia", Code = "SK" }
                };
            }

            UploadDialog.RefreshRequested += LoadItem;

            return base.Init();
        }

        public override async Task PreRender()
        {
            if (!Context.IsPostBack)
            {
                await LoadItem();
            }

            await base.PreRender();
        }

        private async Task LoadItem()
        {
            Item = await client.ApiOrdersByOrderIdGetAsync(ItemId);
        }
        
        public async Task SaveAddress()
        {
            await ValidationHelper.Call(Context, async () =>
            {
                await client.ApiOrdersUpdateAddressByIdPostAsync(ItemId, Item.BillingAddress);
                await LoadItem();
                EditMode = OrderEditMode.ReadOnly;
            });
        }

        public async Task SaveCustomerData()
        {
            await ValidationHelper.Call(Context, async () =>
            {
                await client.ApiOrdersUpdateCustomerdataByIdPostAsync(ItemId, Item.CustomerData);
                await LoadItem();
                EditMode = OrderEditMode.ReadOnly;
            });
        }

        public async Task SavePaidDate()
        {
            await ValidationHelper.Call(Context, async () =>
            {
                await client.ApiOrdersUpdatePaiddateByIdPostAsync(ItemId, Item.PaymentData.PaidDate);
                await LoadItem();
                EditMode = OrderEditMode.ReadOnly;
            });
        }

        public void Edit(OrderEditMode mode)
        {
            EditMode = mode;
        }

        public async Task CancelEdit(OrderEditMode mode)
        {
            await LoadItem();
            EditMode = OrderEditMode.ReadOnly;
        }


        public async Task DownloadDocument(string url, OrderDocumentDTOType type)
        {
            using (var file = await client.ApiOrdersDocumentByIdByUrlGetAsync(ItemId, url))
            {
                Context.ReturnFile(file.Stream, ItemId + "-" + type + ".pdf", "application/octet-stream");
            }
        }

        public async Task RemoveDocument(string url)
        {
            await client.ApiOrdersDocumentByIdByUrlDeleteAsync(ItemId, url);
            await LoadItem();
        }

    }

    public class CountryDTO
    {
        public string Name { get; set; }
        public string Code { get; set; }
    }

    public enum OrderEditMode
    {
        ReadOnly,
        Address,
        CustomerData,
        PaymentData
    }
}

