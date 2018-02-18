using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotVVM.Framework.ViewModel;
using Microsoft.AspNetCore.Authentication;
using OpenEvents.Client;
using OpenEvents.Frontend.Common.Helpers;

namespace OpenEvents.Public.ViewModels
{
    public class RegisterPaidViewModel : MasterPageViewModel
    {
        private readonly IEventsApi eventsApi;
        private readonly IOrdersApi ordersApi;


        public CreateOrderDTO Order { get; set; }

        [Bind(Direction.ServerToClientFirstRequest)]
        public List<CountryDTO> Countries { get; set; }

        public ObservableCollection<EventPriceDTO> EventPrices { get; set; }

        public string TotalPrice { get; set; }

        public string TotalPriceInclVat { get; set; }


        [FromRoute("Id")]
        public string EventId { get; set; }


        public RegisterPaidViewModel(IEventsApi eventsApi, IOrdersApi ordersApi)
        {
            this.eventsApi = eventsApi;
            this.ordersApi = ordersApi;
        }

        public override async Task Init()
        {
            if (!Context.IsPostBack)
            {
                Countries = new List<CountryDTO>()
                {
                    new CountryDTO() { Name = "Czech Republic", Code = "CZ" },
                    new CountryDTO() { Name = "Slovakia", Code = "SK" }
                };

                EventPrices = (await eventsApi.ApiEventsByIdGetAsync(EventId)).Prices;

                Order = new CreateOrderDTO()
                {
                    BillingAddress = new AddressDTO()
                    {
                        CountryCode = "CZ"
                    },
                    CustomerData = new OrderCustomerDataDTO(),
                    OrderItems = new ObservableCollection<CreateOrderItemDTO>()
                    {
                        CreateAttendee()
                    },
                    ExtensionData = new ObservableCollection<ExtensionDataDTO>()
                };
                await Recalculate();
            }

            await base.Init();
        }

        private CreateOrderItemDTO CreateAttendee()
        {
            return new CreateOrderItemDTO()
            {
                ExtensionData = new ObservableCollection<ExtensionDataDTO>(),
                Sku = EventPrices.FirstOrDefault()?.Sku
            };
        }

        public async Task AddAttendee()
        {
            Order.OrderItems.Add(CreateAttendee());
            await Recalculate();
        }

        public async Task RemoveAttendee(CreateOrderItemDTO item)
        {
            Order.OrderItems.Remove(item);
            await Recalculate();
        }

        public async Task Recalculate()
        {
            var calculationData = new CalculateOrderDTO()
            {
                BillingAddress = new CalculateAddressDTO()
                {
                    CountryCode = Order.BillingAddress.CountryCode,
                    VatNumber = Order.BillingAddress.VatNumber
                },
                OrderItems = new ObservableCollection<CalculateOrderItemDTO>(Order.OrderItems.Select(i => new CalculateOrderItemDTO()
                {
                    Amount = 1,
                    Sku = i.Sku
                }))
            };

            var result = await ordersApi.ApiOrdersCalculateByEventIdPostAsync(EventId, calculationData);
            TotalPrice = $"{result.Price:n0} {result.CurrencyCode}";
            TotalPriceInclVat = $"{result.PriceInclVat:n0} {result.CurrencyCode}";
        }

        public async Task SubmitOrder()
        {
            await ValidationHelper.Call(Context, async () =>
            {
                await ordersApi.ApiOrdersCreateByEventIdPostAsync(EventId, Order);
            });

            Context.RedirectToRoute("default");
        }
    }

    public class CountryDTO
    {
        public string Name { get; set; }
        public string Code { get; set; }
    }
}

