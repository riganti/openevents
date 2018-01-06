using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using OpenEvents.Backend.Model;

namespace OpenEvents.Backend.Services
{
    public class DefaultVatValidator : IVatValidator
    {
        public bool IsValidVat(CalculateAddressDTO address)
        {
            var vat = address.VatNumber;
            if (string.IsNullOrEmpty(vat))
            {
                return true;
            }
            vat = string.Concat(vat.Where(char.IsLetterOrDigit).ToArray());

            // parse the number
            var match = Regex.Match(vat, @"^([A-Z]{2})([0-9]+)$");
            if (match.Success)
            {
                try
                {
                    using (var wc = new CookieAwareWebClient() { Encoding = System.Text.Encoding.UTF8 })
                    {
                        // get form page
                        wc.SetUserAgentAndAcceptHeaders();
                        wc.DownloadString("http://ec.europa.eu/taxation_customs/vies/vatRequest.html");

                        // select member state
                        wc.SetUserAgentAndAcceptHeaders();
                        wc.Headers["Content-Type"] = "application/json; charset=UTF-8";
                        wc.Headers["X-Requested-With"] = "XMLHttpRequest";
                        wc.UploadString("http://ec.europa.eu/taxation_customs/vies/generateForm.html", @"{""ms"": """ + match.Groups[1].Value + @"""}");

                        // validate using the web service
                        wc.SetUserAgentAndAcceptHeaders();
                        wc.Headers["Content-Type"] = "application/x-www-form-urlencoded";

                        var postData = $"memberStateCode={match.Groups[1].Value}&number={match.Groups[2].Value}&traderName=&traderStreet=&traderPostalCode=&traderCity=&requesterMemberStateCode=&requesterNumber=&action=check&check=Ov%C4%9B%C5%99it";
                        var response = wc.UploadString("http://ec.europa.eu/taxation_customs/vies/vatResponse.html", postData);

                        if (response.Contains(@"<span class=""validStyle"">Ano, platné DIČ</span>"))
                        {
                            return true;
                        }
                    }
                }
                catch (Exception)
                {
                }
            }

            return false;

        }

        public class CookieAwareWebClient : WebClient
        {

            private Uri lastUri;
            /// <summary>
            /// Gets or sets whether the Referrer header should be set automatically.
            /// </summary>
            public bool AutoSetReferer { get; set; }

            /// <summary>
            /// Gets or sets whether we shall use one shared cookie container (may be needed for multiple domain sessions).
            /// </summary>
            public bool UseSharedCookieContainer { get; set; }
            
            /// <summary>
            /// Returns a <see cref="T:System.Net.WebRequest"/> object for the specified resource.
            /// </summary>
            protected override WebRequest GetWebRequest(Uri address)
            {
                if (lastUri != null && AutoSetReferer)
                {
                    Headers["Referer"] = lastUri.ToString();
                }
                lastUri = address;

                WebRequest request = base.GetWebRequest(address);
                if (request is HttpWebRequest)
                {
                    (request as HttpWebRequest).AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                    (request as HttpWebRequest).CookieContainer = GetCookieContainer(address.Host);
                }
                return request;
            }

            /// <summary>
            /// Gets the cookie container.
            /// </summary>
            private CookieContainer GetCookieContainer(string domain)
            {
                if (UseSharedCookieContainer)
                {
                    domain = string.Empty;
                }

                lock (cookieContainersLocker)
                {
                    CookieContainer container;
                    if (!cookieContainers.TryGetValue(domain, out container))
                    {
                        container = new CookieContainer();
                        cookieContainers.Add(domain, container);
                    }
                    return container;
                }
            }

            private object cookieContainersLocker = new object();
            private Dictionary<string, CookieContainer> cookieContainers = new Dictionary<string, CookieContainer>();



            /// <summary>
            /// Sets standard user agent and accept headers.
            /// </summary>
            public void SetUserAgentAndAcceptHeaders()
            {
                Headers["Accept"] = "text/html, application/xhtml+xml, */*";
                Headers["Accept-Language"] = "cs-CZ";
                Headers["User-Agent"] = "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; WOW64; Trident/5.0)";
                Headers["Accept-Encoding"] = "gzip, deflate";
            }

            /// <summary>
            /// Uploads a specified collection of form field values
            /// </summary>
            public string UploadString(string url, Dictionary<string, string> request, bool useMultipart = false)
            {
                string data;
                if (!useMultipart)
                {
                    Headers["Content-Type"] = "application/x-www-form-urlencoded";
                    data = string.Join("&", request.Select(r => r.Key + "=" + WebUtility.UrlEncode((string)r.Value)));
                }
                else
                {
                    Headers["Content-Type"] = "multipart/form-data; boundary=---------------------------7dc76301361878";
                    data = string.Join("\r\n", request.Select(r => "-----------------------------7dc76301361878\r\nContent-Disposition: form-data; name=\"" + r.Key + "\"\r\n\r\n" + r.Value));
                    data += "-----------------------------7dc76301361878--\r\n";
                }

                return UploadString(url, data);
            }

        }
    }
}