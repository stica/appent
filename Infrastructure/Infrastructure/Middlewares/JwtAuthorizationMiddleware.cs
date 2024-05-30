using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using Start.Infrastructure.Entites;
using Start.Infrastructure.Interfaces;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Start.Infrastructure.Middlewares
{
    public class JwtAuthorizationMiddleware
    {
        protected readonly Logger _logger;
        private IAuthenticationService _authenticationService;
        private readonly RequestDelegate _next;
        private SecurityKey _signingKey;

        public JwtAuthorizationMiddleware(IAuthenticationService authenticationService, RequestDelegate next, AppSettings appSettings)
        {
            _logger = Logger.Create(GetType().FullName);
            _authenticationService = authenticationService;
            _next = next;
            _signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSettings.SharedSecret));
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var requestHeaders = httpContext.Request.Headers;

            string ipAddress = requestHeaders.FirstOrDefault(x => x.Key.ToLower() == "x-forwarder-for").Value.ToString();

            if (string.IsNullOrEmpty(ipAddress))
            {
                ipAddress = httpContext.Connection.RemoteIpAddress.ToString();
            }

            var ipAddressArray = ipAddress.Split(',');
            if (ipAddressArray.Length > 1)
            {
                _logger.Info($"Multiple Ip Addresses for User. Value: {ipAddress}");
                ipAddress = ipAddressArray.Last().Trim();
            }

            var clientCurrentDate = requestHeaders.FirstOrDefault(x => x.Key.ToLower() == "x-current-date").Value.ToString();
            
            if (string.IsNullOrEmpty(clientCurrentDate))
            {
                clientCurrentDate = "0";
            }

            DateTime? clientLocalDateTime = null;

            if (string.IsNullOrEmpty(clientCurrentDate))
            {
                var from = new DateTime(1970, 1, 1);
                clientLocalDateTime = from.AddMilliseconds(Convert.ToInt64(clientCurrentDate));
            }

            var value = httpContext.Request.Cookies["cookie-value"];

            var isSessionValid = _authenticationService.CheckSessionToken(value, ipAddress);

            JwtToken identity = null;
            var authenticationHeader = GetAuthorizationHeader(httpContext);

            if (isSessionValid)
            {
                _logger.Info("Valid session, create identity");

                JwtSecurityToken userPayloadToken = GenerateUserClaimFromJwt(authenticationHeader, ipAddress);
                
                if (userPayloadToken != null)
                {
                    identity = GetUSerIdentity(userPayloadToken);
                    string[] roles = { "ALL" };
                    var genericPrincipal = new GenericPrincipal(identity, roles);
                    httpContext.User = genericPrincipal;
                }
            }

            if (identity != null)
            {
                UserContext.CreateUserContext(identity.UserId, identity.CompanyId, ipAddress, identity.IsAdmin);
                await _next.Invoke(httpContext);
                UserContext.InvalidateUserContext();
                return;
            }

            _logger.Info("create anonymous identity");

            UserContext.CreateAnonymousUserContext(ipAddress);
            await _next.Invoke(httpContext);
            UserContext.InvalidateUserContext();
        }

        private JwtSecurityToken GenerateUserClaimFromJwt(string authToken, string ipAddress)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidAudience = "publicApp",
                ValidIssuer = "publicApp",
                IssuerSigningKey = _signingKey,
                ClockSkew = TimeSpan.FromSeconds(5)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken validatedToken;

            try
            {
                tokenHandler.ValidateToken(authToken.Replace("Bearer ", string.Empty), tokenValidationParameters, out validatedToken);
                return validatedToken as JwtSecurityToken;

            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }

            return null;
        }

        private string GetAuthorizationHeader(HttpContext context)
        {
            string requestToken = context.Request.Headers["Authorization"];

            return requestToken;
        }

        private JwtToken GetUSerIdentity(JwtSecurityToken userPayloadToken)
        {
            return new JwtToken
            {
                UserId = Convert.ToInt32(userPayloadToken.Claims.FirstOrDefault(x => x.Type == "UserId").Value),
                CompanyId = Convert.ToInt32(userPayloadToken.Claims.FirstOrDefault(x => x.Type == "CompanyId").Value),
                UserName = userPayloadToken.Claims.FirstOrDefault(x => x.Type == "UserName").Value,
                IsAdmin = Convert.ToBoolean(userPayloadToken.Claims.FirstOrDefault(x => x.Type == "IsAdmin").Value),
            };
        }
    }
}
