using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotVVM.Framework.ViewModel;
using OpenEvents.Admin.Helpers;
using OpenEvents.Client;

namespace OpenEvents.Admin.ViewModels
{
    public class EventDetailViewModel : MasterPageViewModel
    {
        private readonly EventsApi client;

        public EventDetailViewModel(EventsApi client)
        {
            this.client = client;
        }

        public override string CurrentSection => "Events";

        public EventDTO Item { get; set; }

        [FromRoute("id")]
        public string ItemId { get; private set; }


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
            if (ItemId == null)
            {
                Item = new EventDTO()
                {
                    Title = "New Event",
                    MaxAttendeeCount = 150,
                    RegistrationBeginDate = DateTime.Today,
                    RegistrationEndDate = DateTime.Today.AddMonths(2),
                    Dates = new ObservableCollection<EventDateDTO>(),
                    Prices = new ObservableCollection<EventPriceDTO>(),
                    CancellationPolicies = new ObservableCollection<EventCancellationPolicyDTO>()
                };
            }
            else
            {
                Item = await client.ApiEventsByIdGetAsync(ItemId);
            }
        }

        public async Task Save()
        {
            await ValidationHelper.Call(Context, async () =>
            {
                if (ItemId == null)
                {
                    await client.ApiEventsPostAsync(Item);
                }
                else
                {
                    await client.ApiEventsByIdPutAsync(ItemId, Item);
                }
            });

            Context.RedirectToRoute("EventList");
        }

        public void AddDate()
        {
            Item.Dates.Add(new EventDateDTO());
        }

        public void RemoveDate(EventDateDTO date)
        {
            Item.Dates.Remove(date);
        }

        public void AddPrice()
        {
            Item.Prices.Add(new EventPriceDTO()
            {
                BeginDate = Item.RegistrationBeginDate,
                EndDate = Item.RegistrationEndDate,
                CurrencyCode = "EUR",
                Sku = Guid.NewGuid().ToString()
            });
        }

        public void RemovePrice(EventPriceDTO price)
        {
            Item.Prices.Remove(price);
        }

        public void AddCancellationPolicy()
        {
            Item.CancellationPolicies.Add(new EventCancellationPolicyDTO()
            {
                BeginDate = Item.RegistrationBeginDate,
                EndDate = Item.RegistrationEndDate
            });
        }

        public void RemoveCancellationPolicy(EventCancellationPolicyDTO policy)
        {
            Item.CancellationPolicies.Remove(policy);
        }
    }
}

