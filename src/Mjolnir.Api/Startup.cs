using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Mjolnir.Api.Configurations;
using Mjolnir.Api.Infrastructure;
using Mjolnir.Api.Services;

namespace Mjolnir.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddHttpContextAccessor();
            services.AddScoped<ICurrentHeroService, CurrentHeroService>();
            services.Configure<BifrostConfiguration>(Configuration.GetSection(BifrostConfiguration.Key));
            services.AddAuthentication(defaultScheme: JwtBearerDefaults.AuthenticationScheme) //use authentication services (use JwtBearer by default/fallback)
                    .AddJwtBearer(options => //add the 'jwt bearer' scheme -  scheme is a name which corresponds to an authentication handler (+ its options)
                    {
                        var bifrostConfig = Configuration
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

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
        private byte[] GetBytes(string secret) => Encoding.UTF8.GetBytes(secret);
    }
}
