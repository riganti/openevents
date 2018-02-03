using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DotVVM.Framework.ViewModel;
using DotVVM.Framework.Hosting;
using OpenEvents.Client;

namespace OpenEvents.Public.ViewModels
{
    public class DefaultViewModel : MasterPageViewModel
    {
        private readonly IEventsApi eventsApi;


        public ObservableCollection<EventDTO> FreeEvents { get; set; }

        public ObservableCollection<EventDTO> PaidEvents { get; set; }


        public DefaultViewModel(IEventsApi eventsApi)
        {
            this.eventsApi = eventsApi;
        }


        public override async Task PreRender()
        {
            FreeEvents = await eventsApi.ApiEventsGetAsync(EventType.Free);
            PaidEvents = await eventsApi.ApiEventsGetAsync(EventType.Paid);

            await base.PreRender();
        }
    }
}
