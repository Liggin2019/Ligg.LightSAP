using Microsoft.IdentityModel.Tokens;
using System;
using System.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Ligg.Infrastructure.Helpers;
using Ligg.Infrastructure.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace Ligg.Uwa.Application.Shared
{
    public class JwtHelper
    {
        public static string GenerateToken(OperatorInfo oprt)
        {
            var systemSetting = GlobalContext.SystemSetting;
            var claims = new[] {
                    new Claim(ClaimTypes.Sid,oprt.Id.ToString()),
                    new Claim(ClaimTypes.NameIdentifier,oprt.Account),
                    new Claim(ClaimTypes.Name,oprt.Name),
                    new Claim(JwtRegisteredClaimNames.Exp, $"{new DateTimeOffset(DateTime.Now.AddHours(systemSetting.JwtExpiringHours)).ToUnixTimeSeconds()}"),
                    new Claim(JwtRegisteredClaimNames.Nbf, $"{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}")
                };

            var key = new SymmetricSecurityKey(systemSetting.JwtSecurityKey.ConvertStringToBytesByEncoding());
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var securityToken = new JwtSecurityToken(
                issuer: systemSetting.JwtIssuer,
                audience: systemSetting.JwtAudience,
                claims: claims,
                expires: DateTime.Now.AddHours(systemSetting.JwtExpiringHours),
                signingCredentials: creds);

            var token = new JwtSecurityTokenHandler().WriteToken(securityToken);
            return token;
        }
        public static string FindClaim(string token, string oprtClaim = null)
        {
            if(token.IsNullOrEmpty())
                return string.Empty;
            var jwt = new JwtSecurityToken(token).Claims;
            if (oprtClaim.IsNullOrEmpty()) return jwt.FirstOrDefault(x => x.Type == ClaimTypes.Sid).Value;
            if (oprtClaim.ToLower() == "id") return jwt.FirstOrDefault(x => x.Type == ClaimTypes.Sid).Value;
            else if (oprtClaim.ToLower() == "account") return jwt.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
            else if (oprtClaim.ToLower() == "name") return jwt.FirstOrDefault(x => x.Type == ClaimTypes.Name).Value;
            return string.Empty;
        }
        public static string GetToken(HttpRequest request)
        {
            if (request == null) return null;
            string auth = request.Headers["Authorization"].ToString();
            if (auth.IsNullOrEmpty()) return string.Empty;
            var authArr = auth.Split(' ').Trim().Wash();
            if(authArr==null) return string.Empty;
            if(authArr.Length==1) return string.Empty;
            return authArr[1];
        }


    }
}
