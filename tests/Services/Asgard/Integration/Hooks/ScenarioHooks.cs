using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Asgard;
using Asgard.Configurations;
using TechTalk.SpecFlow;
using Xunit.Abstractions;

namespace Integration.Hooks
{
    [Binding]
    public class ScenarioHooks
    {
        public const string BifrostSecretContextKey = "BifrostSecret";
        public const string HttpClientContextKey = "HttpClient";

        [BeforeScenario]
        public static void SetupScenarioContext(ScenarioContext context, ITestOutputHelper outputHelper)
        {
            var apiHost = new WebApplicationFactory<Startup>().WithWebHostBuilder(builder =>
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
            context[HttpClientContextKey] = apiHost.CreateClient();

            //get the bifrost secret used for jwt encry/decry 
            //so we can create a jwt with the same secret
            var secret = apiHost.Services
                      .GetRequiredService<IConfiguration>()
                      .GetSection(BifrostConfiguration.Key)
                      .Get<BifrostConfiguration>()
                      .Secret;

            context[BifrostSecretContextKey] = secret;
        }
    }
}
