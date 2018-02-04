using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotVVM.Framework.ViewModel;
using OpenEvents.Admin.Model;
using OpenEvents.Client;

namespace OpenEvents.Admin.ViewModels
{
    public class MailTemplateListViewModel : MasterPageViewModel
    {

        private readonly IMailingApi client;

        public MailTemplateListViewModel(IMailingApi client)
        {
            this.client = client;
        }

        public override string CurrentSection => "Mail Templates";

        public MailTemplateFilterDTO Filter { get; set; } = new MailTemplateFilterDTO();

        public ObservableCollection<MailTemplateDTO> Items { get; set; }


        public override async Task PreRender()
        {
            Items = await client.ApiMailtemplatesGetAsync(Filter.Intent, Filter.EventId, Filter.LanguageCode);

            await base.PreRender();
        }

        public async Task DeleteItem(string id)
        {
            await client.ApiMailtemplatesByIdDeleteAsync(id);
        }

    }
}

