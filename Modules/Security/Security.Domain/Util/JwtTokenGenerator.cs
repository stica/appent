using Microsoft.IdentityModel.Tokens;
using Start.Infrastructure.Entites;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Security.Domain.Util
{
    public class JwtTokenGenerator
    {
        private readonly string _secretKey;

        public JwtTokenGenerator(AppSettings appSettings)
        {
            _secretKey = appSettings.SharedSecret;
        }

        public string CreateJwtToken(
            string userId,
            string userName,
            string companyId,
            bool isAdmin)
        {
            var claims = new List<Claim>();

            claims.Add(new Claim("UserId", userId));
            claims.Add(new Claim("UserName", userName));
            claims.Add(new Claim("CompanyId", companyId));
            claims.Add(new Claim("IsAdmin", isAdmin.ToString()));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                new JwtHeader(credentials),
                new JwtPayload(
                        issuer: "publicApp",
                        audience: "publicApp",
                        claims: claims,
                        expires: DateTime.UtcNow.Add(new TimeSpan(0, 0, 2)),
                        notBefore: DateTime.UtcNow
                    )
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
