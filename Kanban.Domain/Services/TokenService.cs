using Kanban.Domain.Entities;
using Kanban.Domain.Services;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Kanban.Domain.Interfaces.Services
{
    public class TokenService : ITokenService
    {
        private readonly AppSettings _appSetting;

        public TokenService(IOptions<AppSettings> appSetting)
        {
            _appSetting = appSetting.Value;
        }

        public string Generate(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSetting.JwtKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Login),
                new Claim(ClaimTypes.Email, user.Login)
            };

            var expires = DateTime.UtcNow.AddMinutes(_appSetting.JwtTokenTimeInMinutes);
            var token = new JwtSecurityToken(_appSetting.JwtIssuer, _appSetting.JwtAudience,
                claims, expires: expires, signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
