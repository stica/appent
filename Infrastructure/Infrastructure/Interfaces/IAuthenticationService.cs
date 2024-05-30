using System;
using System.Collections.Generic;
using System.Text;

namespace Start.Infrastructure.Interfaces
{
    public interface IAuthenticationService
    {
        bool CheckSessionToken(string token, string ipAddress);
    }
}
