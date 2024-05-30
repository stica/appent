using Security.Domain.Managers;
using Start.Infrastructure.Interfaces;

namespace Security.Domain.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private ISecurityManager _securityManager;

        public AuthenticationService(ISecurityManager securityManager)
        {
            _securityManager = securityManager;
        }

        public bool CheckSessionToken(string token, string ipAddress)
        {
            var session = _securityManager.GetLoginSessionAnonymous(token);

            //check session ip address and if not the same log user out

            return session != null;
        }
    }
}
