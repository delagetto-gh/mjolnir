using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Acceptance.Hooks;
using FluentAssertions;
using Microsoft.IdentityModel.Tokens;
using Mjolnir.Api.Infrastructure;
using TechTalk.SpecFlow;

namespace Acceptance.Steps
{
    [Binding]
    public sealed class WieldStepDefinitions
    {
        private readonly ScenarioContext _scenarioContext;
        private const string HeroNameKey = "heroName";
        private const string WorthinessKey = "worthiness";
        private const string AsgardPassKey = "jwt";
        private const string ResultKey = "result";

        public WieldStepDefinitions(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [Given(@"the hero (.*) has been created")]
        public void GivenTheWorthyHeroHasBeenCreated(string heroName)
        {
            _scenarioContext.Set<string>(heroName, HeroNameKey);
        }

        [Given(@"the hero is (.*)")]
        public void AndTheHeroIs(string worthiness)
        {
            _scenarioContext.Set<string>(worthiness, WorthinessKey);
        }

        [Given(@"the hero has obtained their Asgard pass")]
        public void AndTheHeroHasObtainedTheirAsgardPass()
        {
            var hero = _scenarioContext.Get<string>(HeroNameKey);
            var worthiness = _scenarioContext.Get<string>(WorthinessKey);
            var bifrostSecret = _scenarioContext.Get<string>(ScenarioHooks.BifrostSecret);

            // create bifrost pass (JWT) for hero with isworthy claim
            var jwt = GenerateJwt(hero, worthiness, bifrostSecret);

            // sign pass with secret
            _scenarioContext.Set<string>(jwt, AsgardPassKey);
        }

        [Given(@"the hero does not have an Asgard pass")]
        public void ButTheHeroDoesNotHaveAnAsgardPass()
        {
            _scenarioContext.Set<string>("", AsgardPassKey);
        }

        [When(@"the hero attempts to wield Mjolnir")]
        public async Task WhenTheHeroAttemptsToWeildMjolnir()
        {
            var jwt = _scenarioContext.Get<string>(AsgardPassKey);

            using var client = _scenarioContext.Get<HttpClient>(ScenarioHooks.HttpClient);

            var request = new HttpRequestMessage(HttpMethod.Get, "mjolnir")
            {
                Headers =
                {
                    Authorization  = new AuthenticationHeaderValue("Bearer", jwt)
                }
            };

            var response = await client.SendAsync(request);

            _scenarioContext.Set<HttpResponseMessage>(response, ResultKey);
        }

        [Then(@"they should be successful and be deemed worthy")]
        public void ThenTheyShouldBeSuccessfulAndBeDeemedWorthy()
        {
            var response = _scenarioContext.Get<HttpResponseMessage>(ResultKey);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.ReasonPhrase.Should().Be("Worthy");
        }

        [Then(@"they should be unsuccessful and be deemed unworthy")]
        public void ThenTheyShouldBeUnsuccessfulAndBeDeemedUnworthy()
        {
            var response = _scenarioContext.Get<HttpResponseMessage>(ResultKey);
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
            response.ReasonPhrase.Should().Be("Unworthy");
        }

        [Then(@"they should be unsuccessful and be banished from Asgard")]
        public void ThenTheyShouldBeUnsuccessfulAndBeBanishedFromAsgard()
        {
            var response = _scenarioContext.Get<HttpResponseMessage>(ResultKey);
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
            response.ReasonPhrase.Should().Be("Banished");
        }

        private static string GenerateJwt(string heroName, string worthiness, string bifrostSecret)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, heroName),
                new Claim(AsgardianClaims.Worthiness, worthiness)
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
