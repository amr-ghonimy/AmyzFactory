using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace AmyzApi.Helpers
{
    public class TokenManager
    {
        public static string secret = "333322554DSSDFGFDGfcdsfjksdljflkdsjflkdjslfjdslfdsdfsfds";
        public static readonly int DaysOfActiveToken = 30;
        public static string GenerateToken(string userName,string role)
        {
            byte[] key = Convert.FromBase64String(secret);
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(key);

            SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, userName), new Claim(ClaimTypes.Role, role) }),
                Expires = DateTime.UtcNow.AddDays(DaysOfActiveToken),
                SigningCredentials=new SigningCredentials(securityKey,
                SecurityAlgorithms.HmacSha256Signature)
            };

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            JwtSecurityToken token = handler.CreateJwtSecurityToken(descriptor);

            return handler.WriteToken(token);
        }


        public static ClaimsPrincipal GetPrincipal(string token)
        {
            try
            {
                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                JwtSecurityToken jwtToken = (JwtSecurityToken) tokenHandler.ReadToken(token);
                if (jwtToken == null)
                {
                    return null;
                }
                byte[] key = Convert.FromBase64String(secret);
                TokenValidationParameters parameters = new TokenValidationParameters()
                {
                    RequireExpirationTime = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
                SecurityToken securityToken;
                ClaimsPrincipal principal = tokenHandler.ValidateToken(token,
                    parameters, out securityToken);

                return principal;
            }
            catch (Exception e)
            {

               return null;
            }
        }
        public static string GetRoleByToken(string token)
        {
            string role = null;

             ClaimsPrincipal principal = GetPrincipal(token);
            if (principal == null)
            {
                return null;
            }
            ClaimsIdentity identity = null;
            try
            {
                identity = (ClaimsIdentity)principal.Identity;
            }
            catch (Exception)
            {

                return null;
            }

            Claim userNameClaim = identity.FindFirst(ClaimTypes.Role);
            role = userNameClaim.Value;

            return role;
        }

        public static string validToken(string token)
        {
            string userName = null;
            ClaimsPrincipal principal = GetPrincipal(token);
            if (principal == null)
            {
                return null;
            }
            ClaimsIdentity identity = null;
            try
            {
                identity = (ClaimsIdentity)principal.Identity;
            }
            catch (Exception)
            {

                return null;
            }

            Claim userNameClaim = identity.FindFirst(ClaimTypes.Name);
            userName = userNameClaim.Value;

            return userName;
        }
    }
}