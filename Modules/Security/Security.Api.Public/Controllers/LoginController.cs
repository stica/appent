using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Security.Api.Public.Contract.Requests;
using Security.Api.Public.Contract.Responses;
using Security.Domain.Contract.Commands;
using Security.Domain.Managers;
using Start.Infrastructure;
using System;
using System.Threading.Tasks;

namespace Security.Api.Public.Controllers
{
    [Route("api/login")]
    [ApiController]
    public class LoginController : BaseController
    {
        private readonly ISecurityManager _securityManager;

        public LoginController(ISecurityManager securityManager)
        {
            _securityManager = securityManager;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequest)
        {
            if (string.IsNullOrEmpty(loginRequest.UserName))
            {
                return BadRequest("Username is required fields.");
            }

            if (string.IsNullOrEmpty(loginRequest.Password))
            {
                return BadRequest("Password is required fields.");
            }

            try
            {
                var command = new CreateLoginSession
                {
                    UserName = loginRequest.UserName,
                    Password = loginRequest.Password,
                    IpAddress = UserContext.Current.IpAddress
                };

                bool isPasswordValid = _securityManager.VarifyUserCredentials(command);
                
                if (!isPasswordValid)
                {
                    return Unauthorized("Password do not match.");
                }

                var loginSession = await _securityManager.CreateLoginSession(command);

                if (loginSession == null)
                {
                    return Unauthorized();
                }

                var tokenResult = new TokenResponse
                {
                    AccessToken = loginSession.AccessToken,
                    RefreshToken = loginSession.RefreshToken,
                    RefreshTokenExpiration = loginSession.Expires
                };

                return Ok(tokenResult);
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }

        [AllowAnonymous]
        [Route("refresh")]
        public IActionResult Refresh(string refreshToken)
        {
            var session = _securityManager.GetAccessToken(refreshToken, UserContext.Current.IpAddress);

            if (session == null)
            {
                return Unauthorized();
            }

            var tokenResult = new TokenResponse()
            {
                AccessToken = session.AccessToken,
                RefreshToken = session.RefreshToken,
                RefreshTokenExpiration = session.Expires,
            };

            return Ok(tokenResult);
        }
    }
}
