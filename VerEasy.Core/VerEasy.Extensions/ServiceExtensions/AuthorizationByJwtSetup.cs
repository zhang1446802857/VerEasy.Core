using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using VerEasy.Common.Helper;

namespace VerEasy.Extensions.ServiceExtensions
{
    /// <summary>
    /// 添加jwtBearer认证相关[固定写法]
    /// </summary>
    public static class AuthorizationByJwtSetup
    {
        public static void AddAuthorizationByJwtSetup(this IServiceCollection services)
        {
            string iss = Appsettings.App("JwtSettings:Issuer");
            string aud = Appsettings.App("JwtSettings:Audience");
            string key = Appsettings.App("JwtSettings:Key");
            var signKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

            var signingCredentials = new SigningCredentials(signKey, SecurityAlgorithms.HmacSha256);

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signKey,
                ValidIssuer = iss,
                ValidAudience = aud,
                ValidateLifetime = true,
                RequireExpirationTime = true
            };

            services.AddAuthentication(x =>
            {
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.TokenValidationParameters = tokenValidationParameters;
            });
        }
    }
}