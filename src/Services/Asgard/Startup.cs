using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Asgard.Infrastructure;
using Asgard.Services;

namespace Asgard
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
            services.AddScoped<IMjolnirWieldingService, MjolnirWieldingService>();
            services.AddScoped<IWorthyHerosList, WorthyHerosList>();
            services.AddAuthentication(defaultScheme: JwtBearerDefaults.AuthenticationScheme) //use authentication services (use JwtBearer by default/fallback)
                    .AddAsgardPass(Configuration); //add custom Asgard pass auth scheme (jwt really)
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthentication(); //add auth middleware

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
