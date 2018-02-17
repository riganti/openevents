using System;
using System.Collections.Generic;
using System.Text;

namespace OpenEvents.Backend.Common.Configuration
{
    public class SmtpConfiguration
    {

        public string Directory { get; set; }

        public string Host { get; set; }

        public int Port { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

    }
}
