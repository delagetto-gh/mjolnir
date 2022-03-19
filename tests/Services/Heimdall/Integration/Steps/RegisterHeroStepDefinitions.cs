using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Heimdall.Requests;
using Integration.Hooks;
using Newtonsoft.Json;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Integration.Steps
{
    [Binding]
    public sealed class RegisterHeroStepDefinitions
    {
        private readonly ScenarioContext _scenarioContext;
        private const string RequestContextKey = "request";
        private const string ResultContextKey = "result";

        public RegisterHeroStepDefinitions(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [Given("a hero exists with the name (.*)")]
        public async Task GivenAHeroExistsWithName(string heroName)
        {
            var heroToRegister = new RegisterHeroRequest
            {
                HeroName = heroName,
                Password = "abcd1234!"
            };
            var heroToRegisterJson = JsonConvert.SerializeObject(heroToRegister);
            var httpRequest = new HttpRequestMessage(HttpMethod.Post, "/heroes")
            {
                Content = new StringContent(heroToRegisterJson, Encoding.UTF8, "application/json")
            };

            var httpClient = _scenarioContext.Get<HttpClient>(ScenarioHooks.HttpClientContextKey);

            var response = await httpClient.SendAsync(httpRequest);

            response.StatusCode.Should().HaveValue(201, "we need to sucessfully register a hero as a pre-requisite.");
        }

        [Given("I create a (.*) request to (.*)")]
        public void AndICreateARequestTo(string method, string endpoint)
        {
            var httpRequest = new HttpRequestMessage(new HttpMethod(method), endpoint);

            _scenarioContext.Set<HttpRequestMessage>(httpRequest, RequestContextKey);
        }

        [Given("I add the following hero registration details to the request body:")]
        public void AndIAddTheFollowingHerpRegistrationDetailstoTheRequestBody(Table table)
        {
            var heroToRegister = table.CreateInstance<RegisterHeroRequest>();
            var heroToRegisterJson = JsonConvert.SerializeObject(heroToRegister);

            var httpRequest = _scenarioContext.Get<HttpRequestMessage>(RequestContextKey);
            httpRequest.Content = new StringContent(heroToRegisterJson, Encoding.UTF8, "application/json");

            _scenarioContext.Set<HttpRequestMessage>(httpRequest, RequestContextKey);
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

        [Then(@"the response body should contain the error message \""(.*)\""")]
        public async Task AndTheResponseBodyShouldContainTheErrorMessage(string expectedErrorMessage)
        {
            var response = _scenarioContext.Get<HttpResponseMessage>(ResultContextKey);
            var responseBody = await response.Content.ReadAsStringAsync();
            responseBody.Should().Be(expectedErrorMessage);
        }
    }
}
