using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace UCABPagaloTodoWeb.Utilities
{
    public class DecodeToken
    {
        public static string DecodeTokeUsername(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var tokenS = handler.ReadJwtToken(token);

            var claims = tokenS.Claims;

            return claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        }

        public static string DecodeTokeRole(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var tokenS = handler.ReadJwtToken(token);

            var claims = tokenS.Claims;

            return claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
        }
    }
}
