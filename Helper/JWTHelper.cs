
using Microsoft.AspNetCore.DataProtection;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace WebApplication2.Helper
{
    public class JWTHelper
    {
        private readonly byte[] secret = Encoding.ASCII.GetBytes("ADXSADSAD-SADSADASDASD-DSADSADSADSAX");

        public string CrearToken(string username)
        {


            var claims = new ClaimsIdentity();
            claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, @username));
            claims.AddClaim(new Claim(ClaimTypes.GivenName, "Prueba de nombre"));
            for (int i = 0; i < 5; i++)
            {
                claims.AddClaim(new Claim(ClaimTypes.Role, "visitante" + i));
            }


            var tokenDescription = new SecurityTokenDescriptor()
            {
                Subject = claims,
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var createdToken = tokenHandler.CreateToken(tokenDescription);

            return tokenHandler.WriteToken(createdToken);

        }
    }
}