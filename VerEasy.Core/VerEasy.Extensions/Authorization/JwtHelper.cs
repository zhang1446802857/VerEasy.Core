using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using VerEasy.Common.Helper;

namespace VerEasy.Extensions.Authorization
{
    public class JwtHelper
    {
        /// <summary>
        /// 获取token
        /// </summary>
        /// <param name="tokenModel"></param>
        /// <returns></returns>
        public static string IssueJwtToken(TokenModelJwt tokenModel)
        {
            string iss = Appsettings.App("JwtSettings:Issuer");
            string aud = Appsettings.App("JwtSettings:Audience");
            string key = Appsettings.App("JwtSettings:Key");

            //载荷内容,存放用户信息内容等
            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Jti,tokenModel.Id.ToString()),
                new(JwtRegisteredClaimNames.Name,tokenModel.Name.ToString()),
                new(JwtRegisteredClaimNames.Iat,$"{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}"),
                new(JwtRegisteredClaimNames.Nbf,$"{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}"),
                new(JwtRegisteredClaimNames.Exp,$"{new DateTimeOffset(DateTime.Now.AddHours(2)).ToUnixTimeSeconds()}"),
                new(JwtRegisteredClaimNames.Iss,iss),
                new(JwtRegisteredClaimNames.Aud,aud),
            };

            //添加用户信息到载荷,用于通过Token获取用户权限
            claims.AddRange(tokenModel.Role.Split(',').Select(x => new Claim(ClaimTypes.Role, x)));

            //加密key
            var tkey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            //加密方式
            var cred = new SigningCredentials(tkey, SecurityAlgorithms.HmacSha256);

            //写出Token
            var jwt = new JwtSecurityToken(issuer: iss, claims: claims, signingCredentials: cred);
            var jwtToken = new JwtSecurityTokenHandler().WriteToken(jwt);

            return jwtToken;
        }

        /// <summary>
        /// 令牌信息
        /// </summary>
        public class TokenModelJwt
        {
            public long Id { get; set; }
            public string Role { get; set; }
            public string Name { get; set; }
        }
    }
}