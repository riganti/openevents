using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Driver;
using Moq;
using OpenEvents.Backend.Common.Services;
using OpenEvents.Backend.Orders.Data;
using OpenEvents.Backend.Orders.Exceptions;
using OpenEvents.Backend.Orders.Facades;
using OpenEvents.Backend.Orders.Model;
using OpenEvents.Backend.Orders.Queries;
using OpenEvents.Backend.Orders.Services;
using EventDateDTO = OpenEvents.Client.EventDateDTO;
using EventDTO = OpenEvents.Client.EventDTO;
using EventPriceDTO = OpenEvents.Client.EventPriceDTO;

namespace OpenEvents.Tests
{
    [TestClass]
    public class OrderCreateTests
    {

        private OrderCreationFacade CreateFacade(DateTime date)
        {
            var vatRateProvider = new Mock<IVatRateProvider>();
            vatRateProvider.Setup(p => p.GetVatRate(It.IsAny<DateTime>(), It.IsAny<CalculateAddressDTO>())).Returns(1.21m);

            var vatNumberValidator = new Mock<IVatNumberValidator>();
            vatNumberValidator.Setup(v => v.IsValidVat(It.IsAny<CalculateAddressDTO>())).Returns(true);

            var dateTimeProvider = new Mock<IDateTimeProvider>();
            dateTimeProvider.SetupGet(v => v.Now).Returns(date);

            var eventsApi = new Mock<OpenEvents.Client.IEventsApi>();
            eventsApi.Setup(a => a.ApiRegistrationsByEventIdCountGetAsync(It.IsAny<string>())).Returns(Task.FromResult(0));

            var ordersCollection = new Mock<IMongoCollection<Order>>();

            var orderNumbersQuery = new Mock<OrderNumbersQuery>(ordersCollection.Object);
            orderNumbersQuery.Setup(q => q.Execute()).Returns(Task.FromResult<IList<string>>(new List<string>() { "2018000001" }));

            var priceCalculationFacade = new OrderPriceCalculationFacade(vatRateProvider.Object, vatNumberValidator.Object, dateTimeProvider.Object);
            return new OrderCreationFacade(ordersCollection.Object, () => orderNumbersQuery.Object, priceCalculationFacade, dateTimeProvider.Object, eventsApi.Object);
        }


        [TestMethod]
        public async Task OrderCreate_BeforeRegistrationOpen()
        {
            var now = new DateTime(2018, 1, 1);
            var facade = CreateFacade(now);

            var singlePriceEvent = CreateSinglePriceEvent();
            var orderData = CreateSingleAttendeeOrder("sku1");

            await Assert.ThrowsExceptionAsync<RegistrationClosedException>(async () =>
            {
                await facade.CreateOrder(singlePriceEvent, orderData);
            });
        }

        [TestMethod]
        public async Task OrderCreate_AfterRegistrationClosed()
        {
            var now = new DateTime(2018, 3, 1);
            var facade = CreateFacade(now);

            var singlePriceEvent = CreateSinglePriceEvent();
            var orderData = CreateSingleAttendeeOrder("sku1");

            await Assert.ThrowsExceptionAsync<RegistrationClosedException>(async () =>
            {
                await facade.CreateOrder(singlePriceEvent, orderData);
            });
        }

        [TestMethod]
        public async Task OrderCreate_InvalidSku()
        {
            var now = new DateTime(2018, 1, 20);
            var facade = CreateFacade(now);

            var singlePriceEvent = CreateSinglePriceEvent();
            var orderData = CreateSingleAttendeeOrder("skuXXX");

            await Assert.ThrowsExceptionAsync<InvalidSkuException>(async () =>
            {
                await facade.CreateOrder(singlePriceEvent, orderData);
            });
        }

        [TestMethod]
        public async Task OrderCreate_SkuNotOpen()
        {
            var now = new DateTime(2018, 1, 11);
            var facade = CreateFacade(now);

            var singlePriceEvent = CreateSinglePriceEvent();
            var orderData = CreateSingleAttendeeOrder("skuXXX");

            await Assert.ThrowsExceptionAsync<InvalidSkuException>(async () =>
            {
                await facade.CreateOrder(singlePriceEvent, orderData);
            });
        }

        [TestMethod]
        public async Task OrderCreate_ValidOrder()
        {
            var now = new DateTime(2018, 1, 20);
            var facade = CreateFacade(now);

            var singlePriceEvent = CreateSinglePriceEvent();
            var orderData = CreateSingleAttendeeOrder("sku1");

            var order = await facade.CreateOrder(singlePriceEvent, orderData);

            Assert.AreEqual("2018000002", order.Id);
            Assert.AreEqual(100, order.TotalPrice.BasePrice);
            Assert.AreEqual(100, order.TotalPrice.Price);
            Assert.AreEqual(1.21m, order.TotalPrice.VatRate);
            Assert.AreEqual(121, order.TotalPrice.PriceInclVat);
            Assert.AreEqual("EUR", order.TotalPrice.CurrencyCode);
        }




        private static CreateOrderDTO CreateSingleAttendeeOrder(string sku)
        {
            return new CreateOrderDTO()
            {
                BillingAddress = CreateOrderBillingAddress(),
                OrderItems =
                {
                    new CreateOrderItemDTO()
                    {
                        Sku = sku,
                        FirstName = "Test",
                        LastName = "User",
                        Email = "test@mail.com"
                    }
                }
            };
        }

        private static AddressDTO CreateOrderBillingAddress()
        {
            return new AddressDTO()
            {
                Name = "Test Customer",
                Street = "Street",
                City = "City",
                ZIP = "12345",
                CountryCode = "CZ",
                ContactEmail = "test@mail.com",
                ContactPhone = "123456789"
            };
        }

        private static EventDTO CreateSinglePriceEvent()
        {
            return new EventDTO()
            {
                Id = "event",
                Title = "Future event",
                Dates = new ObservableCollection<EventDateDTO>()
                {
                    new EventDateDTO()
                    {
                        BeginDate = new DateTime(2018, 2, 1, 9, 0, 0),
                        EndDate = new DateTime(2018, 2, 1, 17, 0, 0)
                    }
                },
                RegistrationBeginDate = new DateTime(2018, 1, 10, 0, 0, 0),
                RegistrationEndDate = new DateTime(2018, 2, 1, 0, 0, 0),
                Prices = new ObservableCollection<EventPriceDTO>()
                {
                    new EventPriceDTO()
                    {
                        Sku = "sku1",
                        Price = 100,
                        CurrencyCode = "EUR",
                        BeginDate = new DateTime(2018, 1, 15, 0, 0, 0),
                        EndDate = new DateTime(2018, 2, 1, 0, 0, 0)
                    }
                }
            };
        }
    }
}
