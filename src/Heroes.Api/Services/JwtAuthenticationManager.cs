using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Heroes.Api.Options;

namespace Heroes.Api.Services
{
    ///<summary>
    /// Class responsible (userid + password)
    /// password against data store. if the userid + password matches, well create a JWT token
    ///</summary>
    public class JwtAuthenticationManager
    {
        private readonly IDictionary<string, string> _db; //fake db store for user and + password data;
        private readonly JwtOptions _options;

        public JwtAuthenticationManager(IOptions<JwtOptions> options)
        {
            //imagine list of 'regisred' users in a users db
            _db = new Dictionary<string, string>()
            {
                ["delagetto-test1"] = "abcd1234",
                ["delagetto-test2"] = "toto"
            };

            _options = options.Value;
        }

        //auth method
        public string Authenticate(string userName, string password)
        {
            //authentication logic
            if (!_db.Any(o => o.Key == userName && o.Value == password))
                return null;

            //create JWT and return for this user

            //1. create token handler;
            var tokenHandler = new JwtSecurityTokenHandler();

            //2 get bytes of encryp key
            var bytes = Encoding.ASCII.GetBytes(_options.Secret);

            //3 create a token descriptor
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                //Claims are statements about an entity (typically, the user) and additional data. 
                //recommended, to provide a set of useful, interoperable claims. Some of them are: 
                // iss (issuer), 
                // exp (expiration time), 
                // sub (subject - WHO IS IT FAM), 
                // aud (audience - WHOS IT FOR FAM), and others.
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, userName)
                }),
                Expires = DateTime.UtcNow.AddHours(1), //exp after an hour,
                //sign the token with our private encryption key, using HMac256 algorithm
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(bytes),
                                                            SecurityAlgorithms.HmacSha256Signature)
            };

            //4. create the token
            var token = tokenHandler.CreateToken(tokenDescriptor);

            //5. write the token as JSON JWT
            var jwt = tokenHandler.WriteToken(token);

            return jwt; //return the JWT 
        }
    }
}