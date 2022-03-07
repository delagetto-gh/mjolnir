using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Mjolnir.Api;
using Mjolnir.Api.Options;
using TechTalk.SpecFlow;
using Xunit;

namespace Acceptance.Steps
{
    [Binding]
    public sealed class WieldStepDefinitions : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly ScenarioContext _scenarioContext;
        private readonly WebApplicationFactory<Startup> _webApplicationFactory;

        public WieldStepDefinitions(ScenarioContext scenarioContext, WebApplicationFactory<Startup> webapplicationFactory)
        {
            _scenarioContext = scenarioContext;
            _webApplicationFactory = webapplicationFactory;
        }

        [Given(@"the worthy hero (.*)")]
        public void GivenTheWorthyHero(string hero)
        {
            _scenarioContext["hero"] = hero;
            _scenarioContext["isWorthy"] = true;
        }


        [Given(@"the hero has a Bifrost pass")]
        public void AndTheHeroHasABifrostPass()
        {
            var hero = (string)_scenarioContext["hero"];
            var isWorthy = (bool)_scenarioContext["isWorthy"];
            var bifrostOptions = _webApplicationFactory
                            .Services
                            .GetRequiredService<IOptions<BifrostOptions>>()
                            .Value;

            // create bifrost pass (JWT) for hero with isworthy claim
            var jwt = GenerateJwt(hero, isWorthy, bifrostOptions);

            // sign pass with secret
            _scenarioContext["jwt"] = jwt;
        }

        [When(@"the hero attempts to wield Mjolnir")]
        public async Task WhenTheHeroAttemptsToWeildMjolnir()
        {
            var jwt = (string)_scenarioContext["jwt"];

            using var client = _webApplicationFactory.CreateClient();

            var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Get,
                Headers =
                {
                    Authorization  = new AuthenticationHeaderValue("Bearer", jwt)
                }
            };

            var response = await client.SendAsync(request);

            _scenarioContext["response"] = response;
        }

        [Then(@"they should be successful")]
        public void ThenTheyShouldBeSuccessful()
        {
            var response = _scenarioContext["response"] as HttpResponseMessage;
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.ReasonPhrase.Should().Be("Worthy");
        }

        private static string GenerateJwt(string heroName, bool isWorthy, BifrostOptions bifrostOptions)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, heroName),
                new Claim("iwy", isWorthy.ToString())
            };

            var signingKey = new SymmetricSecurityKey(GetBytes(bifrostOptions.Secret));
            
            var secDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                SigningCredentials = new SigningCredentials(signingKey, bifrostOptions.SecurityAlgorithm)
            };

            var jwtTokenHandler = new JwtSecurityTokenHandler();

            var secToken = jwtTokenHandler.CreateToken(secDescriptor);

            var jwt = jwtTokenHandler.WriteToken(secToken);

            return jwt;
        }

        private static byte[] GetBytes(string secret) => Encoding.UTF8.GetBytes(secret);
    }
}
