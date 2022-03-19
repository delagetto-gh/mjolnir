using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Logging;
using TechTalk.SpecFlow;
using Xunit.Abstractions;
using System.Net.Http;
using Heimdall;

namespace Integration.Hooks
{
    [Binding]
    public class ScenarioHooks
    {
        public const string HttpClientContextKey = "httpClient";

        [BeforeScenario]
        public static void SetupScenarioContext(ScenarioContext context, ITestOutputHelper outputHelper)
        {
            var apiHost = new WebApplicationFactory<Startup>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.AddXUnit(outputHelper); //output app logs to xunit output 
                });
                builder.ConfigureTestServices(services =>
                {
                    //mock out any dependencies here if needed.
                });
            });
            
            //set the httpclient for the scenario steps to use
            context.Set<HttpClient>(apiHost.CreateClient(), HttpClientContextKey);
        }
    }
}
