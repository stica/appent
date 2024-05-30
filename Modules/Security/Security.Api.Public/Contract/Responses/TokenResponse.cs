using System;
using System.Collections.Generic;
using System.Text;

namespace Security.Api.Public.Contract.Responses
{
    public class TokenResponse
    {
        public string AccessToken { get; set; }

        public DateTime RefreshTokenExpiration { get; set; }

        public string RefreshToken { get; set; }
    }
}
