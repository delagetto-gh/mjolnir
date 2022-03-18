using System.IO;
using Heimdall.Infrastructure;
using Heimdall.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
namespace Heimdall
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
            var db = "Identities.db";

            services.AddControllers()
                    .AddControllersAsServices();

            services.AddDbContext<IdentityDbContext>(opt => opt.UseSqlite($"Data Source={db}"));
            services.AddScoped<HeroesManagerService>();
            services.AddScoped<IAsgardPassIssuerService>(sp => sp.GetService<HeroesManagerService>());
            services.AddScoped<IHeroRegistrationService>(sp => sp.GetService<HeroesManagerService>());
            services.AddIdentityCore<IdentityUser>()
                    .AddEntityFrameworkStores<IdentityDbContext>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IdentityDbContext dbContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //create the db
            dbContext.Database.EnsureCreated();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
