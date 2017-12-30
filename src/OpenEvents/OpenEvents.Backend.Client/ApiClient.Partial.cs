using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace OpenEvents.Backend.Client
{
    public partial class ApiClient
    {

        partial void UpdateJsonSerializerSettings(Newtonsoft.Json.JsonSerializerSettings settings)
        {
            settings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
        }

    }
}
