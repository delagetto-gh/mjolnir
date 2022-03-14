using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Integration.Hooks;
using FluentAssertions;
using Microsoft.IdentityModel.Tokens;
using TechTalk.SpecFlow;
using Integration.Utilities;

namespace Integration.Steps
{
    [Binding]
    public sealed class WieldMjolnirStepDefinitions
    {
        private readonly ScenarioContext _scenarioContext;
        private const string HeroContextKey = "heroName";
        private const string AsgardPassContextKey = "ap";
        private const string RequestContextKey = "request";
        private const string ResultContextKey = "result";

        public WieldMjolnirStepDefinitions(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [Given(@"I have the hero (.*)")]
        public void GivenIHaveTheHero(string heroName)
        {
            _scenarioContext.Set<string>(heroName, HeroContextKey);
        }

        [Given(@"I have my hero's AP")]
        public void AndIHaveMyHerosAp()
        {
            var hero = _scenarioContext.Get<string>(HeroContextKey);

            var apGenerator = _scenarioContext.Get<AsgardPassGenerator>(ScenarioHooks.AsgardPassGeneratorContextKey);

            var ap = apGenerator.Generate(hero);

            _scenarioContext.Set<string>(ap, AsgardPassContextKey);
        }

        [Given(@"I create a (.*) request to (.*)")]
        public void AndICreateARequestTo(string method, string endpoint)
        {
            var request = new HttpRequestMessage(new HttpMethod(method), endpoint);

            _scenarioContext.Set<HttpRequestMessage>(request, RequestContextKey);
        }

        [Given(@"I add in the authorisation header (.*)")]
        public void AndIAddInTheAuthorisationHeader(string authHeaderTemplate)
        {
            var authScheme = authHeaderTemplate.Split(' ')[0];
            var authValue = _scenarioContext.Get<string>(AsgardPassContextKey);

            var request = _scenarioContext.Get<HttpRequestMessage>(RequestContextKey);

            request.Headers.Authorization = new AuthenticationHeaderValue(authScheme, authValue);
        }

        [When(@"I make the request")]
        public async Task WhenIMakeTheRequest()
        {
            var request = _scenarioContext.Get<HttpRequestMessage>(RequestContextKey);

            using var client = _scenarioContext.Get<HttpClient>(ScenarioHooks.HttpClientContextKey);

            var response = await client.SendAsync(request);

            _scenarioContext.Set<HttpResponseMessage>(response, ResultContextKey);
        }

        [Then(@"the response status code should be (\d+)")]
        public void ThenTheResponseStatusCodeShouldBe(int statusCode)
        {
            var response = _scenarioContext.Get<HttpResponseMessage>(ResultContextKey);
            response.StatusCode.Should().HaveValue(statusCode);
        }

        [Then(@"the response reason phrase should be (.*)")]
        public void AndTheResponseReasonPhraseShouldBe(string reasonPhrase)
        {
            var response = _scenarioContext.Get<HttpResponseMessage>(ResultContextKey);
            response.ReasonPhrase.Should().Be(reasonPhrase);
        }
    }
}
