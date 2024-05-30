using System.Linq;
using Security.Domain.Managers;
using Start.Infrastructure;
using Start.Infrastructure.Interfaces;

namespace Security.Domain.Services
{
    public class AuthorizationService : IAuthorizationService
    {
        private ISecurityManager _securityManager;

        public AuthorizationService(ISecurityManager securityManager)
        {
            _securityManager = securityManager;
        }

        public bool Authorize(string module, string action)
        {
            var userId = UserContext.Current.UserId;
            var userPolicies = _securityManager.GetUserPolicies(userId);

            return userPolicies.Any(x => x.Document.CheckPolicy(module, action));
        }
    }
}
