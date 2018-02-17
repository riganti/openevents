using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using OpenEvents.Backend.Mailing.Common;
using OpenEvents.Backend.Mailing.Model;
using OpenEvents.Client;
using RazorLight;

namespace OpenEvents.Backend.Mailing.Services
{
    public class TemplateProcessor
    {
        private readonly RazorLightEngine engine;
        private readonly MD5 md5;

        public TemplateProcessor()
        {
            engine = new RazorLightEngineBuilder()
                .UseMemoryCachingProvider()
                .Build();

            md5 = MD5.Create();
        }

        public async Task<string> TransformBody<T>(MailTemplateDTO template, T data)
        {
            var templateKey = GetMd5(template.Body);
            return await engine.CompileRenderAsync(templateKey, template.Body, data);
        }

        private string GetMd5(string template)
        {
            var data = Encoding.UTF8.GetBytes(template);
            var result = md5.ComputeHash(data);
            return Convert.ToBase64String(result).Replace("/", "_").Replace("+", "_");
        }


        public async Task<string> TestTemplate(MailTemplateDTO template)
        {
            if (template.MailIntent == MailIntent.GlobalTemplate)
            {
                return await TestTemplateCore(template, GetGlobalTemplateData());
            }
            else if (template.MailIntent == MailIntent.RegistrationConfirmed)
            {
                return await TestTemplateCore(template, GetRegistrationConfirmedData());
            }
            else if (template.MailIntent == MailIntent.ExternalInvoiceRequest)
            {
                return await TestTemplateCore(template, GetExternalInvoiceRequestData());
            }
            else if (template.MailIntent == MailIntent.ProformaInvoice)
            {
                return await TestTemplateCore(template, (object)null);
            }
            else if (template.MailIntent == MailIntent.Invoice)
            {
                return await TestTemplateCore(template, (object)null);
            }

            return "Cannot validate template!";
        }

        private async Task<string> TestTemplateCore<T>(MailTemplateDTO template, T data)
        {
            try
            {
                return await TransformBody(template, data);
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        private GlobalTemplateDataDTO GetGlobalTemplateData()
        {
            return new GlobalTemplateDataDTO()
            {
                Subject = "Test Email",
                Html = "<p>Test paragraph</p>"
            };
        }


        private OrderDTO GetExternalInvoiceRequestData()
        {
            return new OrderDTO()
            {
                Id = "2018000001",
                EventTitle = "Test Event",
                BillingAddress = new AddressDTO()
                {
                    Name = "Fabrikam",
                    Street = "15 Blue Ave",
                    City = "New York",
                    Zip = "A1234",
                    State = "NY",
                    CountryCode = "USA",
                    CompanyRegistrationNumber = "123456",
                    ContactEmail = "test@mail.com",
                    ContactPhone = "123456789"
                },
                CustomerData = new OrderCustomerDataDTO()
                {
                    Notes = "some notes",
                    OrderNumber = "PO123456"
                },
                OrderItems = new ObservableCollection<OrderItemDTO>()
                {
                    new OrderItemDTO()
                    {
                        Amount = 1,
                        Description = "Full Admission",
                        Price = new PriceDataDTO() { BasePrice = 1000, Price = 1000, PriceInclVat = 1210, CurrencyCode = "CZK", VatRate = 1.21 },
                        Type = OrderItemDTOType.EventPrice
                    },
                    new OrderItemDTO()
                    {
                        Amount = 2,
                        Description = "Early Bird",
                        Price = new PriceDataDTO() { BasePrice = 1000, Price = 2000, PriceInclVat = 2240, CurrencyCode = "CZK", VatRate = 1.21 },
                        Type = OrderItemDTOType.EventPrice
                    },
                    new OrderItemDTO()
                    {
                        Amount = 1,
                        Description = "",
                        Price = new PriceDataDTO() { BasePrice = -500, Price = -500, PriceInclVat = -605, CurrencyCode = "CZK", VatRate = 1.21 },
                        Type = OrderItemDTOType.Discount
                    }
                },
                TotalPrice = new PriceDataDTO() { Price = 2500, PriceInclVat = 3025, CurrencyCode = "CZK", VatRate = 1.21 },
                DiscountCode = "CZ1234556"
            };
        }

        private RegistrationConfirmationDTO GetRegistrationConfirmedData()
        {
            return new RegistrationConfirmationDTO()
            {
                Event = new EventDTO()
                {
                    Id = Guid.NewGuid().ToString(),
                    Title = "Test Event",
                    Description = "<p>This is event description.</p>",
                    MaxAttendeeCount = 150,
                    RegistrationBeginDate = new DateTime(2017, 1, 1),
                    RegistrationEndDate = new DateTime(2018, 1, 1),
                    Dates = new ObservableCollection<EventDateDTO>()
                    {
                        new EventDateDTO() { BeginDate = new DateTime(2018, 1, 1, 9, 0, 0), EndDate = new DateTime(2018, 1, 1, 17, 0, 0) }
                    }
                },
                Registrations = new List<RegistrationDTO>()
                {
                    new RegistrationDTO()
                    {
                        FirstName = "Humphrey",
                        LastName = "Appleby",
                        Email = "humphrey@gov.test",
                        Sku = "full",
                        SkuDescription = "Full Admission"
                    },
                    new RegistrationDTO()
                    {
                        FirstName = "Bernard",
                        LastName = "Woolley",
                        Email = "bernard@gov.test",
                        Sku = "early",
                        SkuDescription = "Early Bird"
                    }
                }
            };
        }
    }
}
