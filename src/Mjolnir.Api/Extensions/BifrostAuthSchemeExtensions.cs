using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Mjolnir.Api.Configurations;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class BifrostAuthSchemeExtensions
    {
        public static void AddBifrost(this AuthenticationBuilder authBuilder, IConfiguration configuration)
        {
            authBuilder.AddJwtBearer(options =>
            {
                var bifrostConfig = configuration
                    .GetSection(BifrostConfiguration.Key)
                    .Get<BifrostConfiguration>();

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
                    IssuerSigningKey = new SymmetricSecurityKey(GetBytes(bifrostConfig.Secret)),
                    ClockSkew = TimeSpan.Zero,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                };
            });
        }

        private static byte[] GetBytes(string secret) => Encoding.UTF8.GetBytes(secret);
    }
}