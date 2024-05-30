using System;
using System.Collections.Generic;
using System.Text;

namespace Start.Infrastructure.Interfaces
{
    public interface IAuthorizationService
    {
        bool Authorize(string module, string action);
    }
}
