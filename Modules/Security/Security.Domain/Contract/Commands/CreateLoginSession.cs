using System;
using System.Collections.Generic;
using System.Text;

namespace Security.Domain.Contract.Commands
{
    public class CreateLoginSession
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string IpAddress { get; set; }
    }
}
