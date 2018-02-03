using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotVVM.Framework.ViewModel;
using OpenEvents.Client;

namespace OpenEvents.Admin.ViewModels
{
    public class EventListViewModel : MasterPageViewModel
    {
        private readonly IEventsApi client;

        public EventListViewModel(IEventsApi client)
        {
            this.client = client;
        }

        public override string CurrentSection => "Events";

        public ObservableCollection<EventDTO> Items { get; set; }


        public override async Task PreRender()
        {
            Items = await client.ApiEventsGetAsync(EventType.All);

            await base.PreRender();
        }

        public async Task DeleteItem(string id)
        {
            await client.ApiEventsByIdDeleteAsync(id);
        }
    }
}

