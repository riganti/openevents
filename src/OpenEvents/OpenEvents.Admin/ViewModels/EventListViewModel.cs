using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotVVM.Framework.ViewModel;
using OpenEvents.Backend.Client;

namespace OpenEvents.Admin.ViewModels
{
    public class EventListViewModel : MasterPageViewModel
    {
        private readonly ApiClient client;

        public EventListViewModel(ApiClient client)
        {
            this.client = client;
        }


        public ObservableCollection<EventDTO> Items { get; set; }


        public override async Task PreRender()
        {
            Items = await client.ApiEventsGetAsync();

            await base.PreRender();
        }

        public async Task DeleteItem(string id)
        {
            await client.ApiEventsByIdDeleteAsync(id);
        }
    }
}

