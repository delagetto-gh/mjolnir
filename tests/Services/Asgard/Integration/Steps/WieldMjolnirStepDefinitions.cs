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

namespace Integration.Steps
{
    [Binding]
    public sealed class WieldMjolnirStepDefinitions
    {
        private readonly ScenarioContext _scenarioContext;
        private const string HeroContextKey = "heroName";
        private const string ApContextKet = "jwt";
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
            var heimdallSecret = _scenarioContext.Get<string>(ScenarioHooks.BifrostSecretContextKey);

            // create bifrost pass (JWT) for hero with isworthy claim
            var jwt = GenerateJwt(hero, heimdallSecret);

            // sign pass with secret
            _scenarioContext.Set<string>(jwt, ApContextKet);
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
            var authValue = _scenarioContext.Get<string>(ApContextKet);

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
            response.StatusCode.Should().Be(statusCode);
        }

        [Then(@"the response reason phrase should be (.*)")]
        public void AndTheResponseReasonPhraseShouldBe(string reasonPhrase)
        {
            var response = _scenarioContext.Get<HttpResponseMessage>(ResultContextKey);
            response.ReasonPhrase.Should().Be(reasonPhrase);
        }
        
        private static string GenerateJwt(string heroName, string bifrostSecret)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, heroName)
            };

            var signingKey = new SymmetricSecurityKey(GetBytes(bifrostSecret));

            var secDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256Signature)
            };

            var jwtTokenHandler = new JwtSecurityTokenHandler();

            var secToken = jwtTokenHandler.CreateToken(secDescriptor);

            var jwt = jwtTokenHandler.WriteToken(secToken);

            return jwt;
        }

        private static byte[] GetBytes(string secret) => Encoding.UTF8.GetBytes(secret);
    }
}
