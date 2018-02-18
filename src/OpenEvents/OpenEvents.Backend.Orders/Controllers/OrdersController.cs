using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpenEvents.Backend.Common.Filters;
using OpenEvents.Backend.Orders.Common;
using OpenEvents.Backend.Orders.Facades;
using OpenEvents.Backend.Orders.Model;
using OpenEvents.Backend.Orders.Services;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace OpenEvents.Backend.Orders.Controllers
{
    [Route("/api/orders")]
    public class OrdersController : Controller
    {
        private readonly OrdersFacade ordersFacade;
        private readonly OrderCreationFacade orderCreationFacade;
        private readonly OrderPriceCalculationFacade orderPriceCalculationFacade;
        private readonly EventsCache eventsCache;
        private readonly DocumentStorageService documentStorageService;


        public OrdersController(OrdersFacade ordersFacade, OrderCreationFacade orderCreationFacade, OrderPriceCalculationFacade orderPriceCalculationFacade, EventsCache eventsCache, DocumentStorageService documentStorageService)
        {
            this.ordersFacade = ordersFacade;
            this.orderCreationFacade = orderCreationFacade;
            this.orderPriceCalculationFacade = orderPriceCalculationFacade;
            this.eventsCache = eventsCache;
            this.documentStorageService = documentStorageService;
        }

        [HttpGet]
        public async Task<List<OrderDTO>> GetList([FromQuery] OrderFilterDTO filter)
        {
            return await ordersFacade.GetAll(filter);
        }

        [HttpGet]
        [Route("{orderId}")]
        public async Task<OrderDTO> GetById(string orderId)
        {
            return await ordersFacade.GetById(orderId);
        }


        [HttpPost]
        [Route("create/{eventId}")]
        public async Task<OrderDTO> Create(string eventId, [FromBody] CreateOrderDTO order)
        {
            var eventData = await eventsCache.Get(eventId);
            return await orderCreationFacade.CreateOrder(eventData, order);
        }

        [HttpPost]
        [Route("calculate/{eventId}")]
        public async Task<PriceDataDTO> Calculate(string eventId, [FromBody] CalculateOrderDTO order)
        {
            var eventData = await eventsCache.Get(eventId);
            var price = await orderPriceCalculationFacade.CalculatePriceForOrderAndItems(eventData, order);
            return price.TotalPrice;
        }

        [HttpPost]
        [Route("update/address/{id}")]
        public async Task<OrderDTO> UpdateAddress(string id, [FromBody] AddressDTO address)
        {
            return await ordersFacade.UpdateAddress(id, address);
        }

        [HttpPost]
        [Route("update/customerdata/{id}")]
        public async Task<OrderDTO> UpdateCustomerData(string id, [FromBody] OrderCustomerDataDTO customerData)
        {
            return await ordersFacade.UpdateCustomerData(id, customerData);
        }

        [HttpPost]
        [Route("update/paiddate/{id}")]
        public async Task<OrderDTO> UpdatePaidDate(string id, DateTime? paidDate)
        {
            return await ordersFacade.UpdatePaidDate(id, paidDate);
        }

        [HttpPost]
        [Route("document/{id}")]
        [SwaggerOperationFilter(typeof(UploadedFileOperationFilter))]
        public async Task<string> UploadDocument(string id, OrderDocumentType type, IFormFile file)
        {
            using (var stream = file.OpenReadStream())
            {
                var url = await documentStorageService.UploadInvoice(stream);

                await ordersFacade.AddDocument(id, type, url);

                return url;
            }
        }

        [HttpGet]
        [Route("document/{id}/{url}")]
        [SwaggerOperationFilter(typeof(FileResponseOperationFilter))]
        public async Task<IActionResult> GetDocument(string id, string url)
        {
            var stream = await documentStorageService.DownloadInvoice(url);
            return File(stream, "application/octet-stream");
        }

        [HttpDelete]
        [Route("document/{id}/{url}")]
        public async Task DeleteDocument(string id, string url)
        {
            await ordersFacade.RemoveDocument(id, url);
        }

    }
}
