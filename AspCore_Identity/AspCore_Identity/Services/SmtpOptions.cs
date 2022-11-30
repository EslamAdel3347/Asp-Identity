using System;
using System.Collections.Generic;
using System.Linq;

namespace AspCore_Identity.Services
{
    public class SmtpOptions
    {
        public int port { get; set; }
        public string Host { get; set; }
        public string  Username { get; set; }
        public string Password { get; set; }
    }
}
