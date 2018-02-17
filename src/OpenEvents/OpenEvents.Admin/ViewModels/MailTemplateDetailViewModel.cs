using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotVVM.Framework.ViewModel;
using OpenEvents.Client;
using OpenEvents.Frontend.Common.Helpers;

namespace OpenEvents.Admin.ViewModels
{
    public class MailTemplateDetailViewModel : MasterPageViewModel
    {
        private readonly IMailingApi client;
        private readonly IEventsApi eventsApi;

        public MailTemplateDetailViewModel(IMailingApi client, IEventsApi eventsApi)
        {
            this.client = client;
            this.eventsApi = eventsApi;
        }

        public override string CurrentSection => "MailTemplates";

        public MailTemplateDTO Item { get; set; }

        [Bind(Direction.ServerToClientFirstRequest)]
        public List<string> Languages { get; set; } = new List<string>()
        {
            "cs",
            "en"
        };

        [Bind(Direction.ServerToClientFirstRequest)]
        public ObservableCollection<EventBasicDTO> Events { get; set; }

        [Bind(Direction.ServerToClientFirstRequest)]
        public List<MailTemplateDTOMailIntent> MailIntents { get; set; } = EnumHelper.CreateCollection<MailTemplateDTOMailIntent>();

        [FromRoute("id")]
        public string ItemId { get; private set; }

        public bool IsTestDisplayed { get; set; }

        public string TestResult { get; set; }


        public override async Task PreRender()
        {
            if (!Context.IsPostBack)
            {
                Events = await eventsApi.ApiEventsBasicGetAsync();

                await LoadItem();
            }

            await base.PreRender();
        }

        private async Task LoadItem()
        {
            if (ItemId == null)
            {
                Item = new MailTemplateDTO();
            }
            else
            {
                Item = await client.ApiMailtemplatesByIdGetAsync(ItemId);
            }
        }

        public async Task Save()
        {
            await ValidationHelper.Call(Context, async () =>
            {
                if (ItemId == null)
                {
                    await client.ApiMailtemplatesPostAsync(Item);
                }
                else
                {
                    await client.ApiMailtemplatesByIdPutAsync(ItemId, Item);
                }
            });

            Context.RedirectToRoute("MailTemplateList");
        }

        public async Task Test()
        {
            TestResult = await client.ApiMailtemplatesTestPostAsync(Item);
            IsTestDisplayed = true;
        }

    }
}

