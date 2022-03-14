using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Asgard.Configurations;
using Microsoft.Extensions.Primitives;
using System.Linq;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class HeimdallExtensions
    {
        public static void AddAsgardPass(this AuthenticationBuilder authBuilder, IConfiguration configuration)
        {
            authBuilder.AddJwtBearer(options =>
            {
                var heimdallConfig = configuration
                    .GetSection(HeimdallConfiguration.Key)
                    .Get<HeimdallConfiguration>();

                options.Events = new JwtBearerEvents
                {
                    OnChallenge = ctx =>
                    {
                        ctx.HandleResponse();
                        ctx.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        ctx.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = "Banished";
                        return Task.CompletedTask;
                    }
                };
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(GetBytes(heimdallConfig.Secret)),
                    ClockSkew = TimeSpan.Zero,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                };
            });
        }

        private static byte[] GetBytes(string secret) => Encoding.UTF8.GetBytes(secret);
    }
}