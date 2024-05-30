using System;
using System.Collections.Generic;
using System.Text;

namespace Security.Api.Public.Contract.Requests
{
    public class LoginRequestDto
    {
        public string UserName { get; set; }

        public string Password { get; set; }
    }
}
