using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Start.Infrastructure
{
    [Authorize]
    public class BaseController : Controller
    {
    }
}
