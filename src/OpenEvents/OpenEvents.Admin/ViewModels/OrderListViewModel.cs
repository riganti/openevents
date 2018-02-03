using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotVVM.Framework.ViewModel;
using OpenEvents.Backend.Orders.Model;
using OpenEvents.Client;

namespace OpenEvents.Admin.ViewModels
{
    public class OrderListViewModel : MasterPageViewModel
    {
        private readonly OrdersApi ordersApi;
        private readonly EventsApi eventsApi;

        public override string CurrentSection => "Orders";


        public OrderFilterDTO Filter { get; set; } = new OrderFilterDTO();

        [Bind(Direction.ServerToClientFirstRequest)]
        public List<EventBasicDTO> Events { get; set; }

        public ObservableCollection<OrderDTO> Items { get; set; }

        public OrderListViewModel(OrdersApi ordersApi, EventsApi eventsApi)
        {
            this.ordersApi = ordersApi;
            this.eventsApi = eventsApi;
        }


        public override async Task Init()
        {
            if (!Context.IsPostBack)
            {
                Events = (await eventsApi.ApiEventsBasicGetAsync()).ToList();
            }

            await base.Init();
        }

        public override async Task PreRender()
        {
            Items = await ordersApi.ApiOrdersGetAsync(Filter.SearchText, Filter.EventId);

            await base.PreRender();
        }
        
    }
}

