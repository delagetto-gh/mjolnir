using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Integration.Utilities
{
    internal class AsgardPassGenerator
    {
        private readonly string _secret;

        public AsgardPassGenerator(string heimdallSecret)
        {
            _secret = heimdallSecret;
        }
        
        internal string Generate(string hero)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, hero)
            };

            var signingKey = new SymmetricSecurityKey(GetBytes(_secret));

            var secDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256Signature)
            };

            var jwtTokenHandler = new JwtSecurityTokenHandler();

            var secToken = jwtTokenHandler.CreateToken(secDescriptor);

            var jwt = jwtTokenHandler.WriteToken(secToken);

            return jwt;
        }

        private static byte[] GetBytes(string secret) => Encoding.UTF8.GetBytes(secret);
    }
}