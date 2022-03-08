using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Logging;
using Mjolnir.Api;
using Xunit.Abstractions;

namespace Acceptance.Fixtures
{
    public class MjolnirApiFixture : WebApplicationFactory<Startup>
    {
        public ITestOutputHelper XunitTestOutputHelper { get; set; }
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureLogging(logging =>
            {
                logging.ClearProviders();
                logging.AddXUnit(XunitTestOutputHelper);
            });
        }
    }
}