using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Acceptance.Fixtures;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Mjolnir.Api;
using Mjolnir.Api.Configurations;
using Mjolnir.Api.Infrastructure;
using TechTalk.SpecFlow;
using Xunit;
using Xunit.Abstractions;

namespace Acceptance.Steps
{
    [Binding]
    public sealed class WieldStepDefinitions : IClassFixture<MjolnirApiFixture>
    {
        private readonly ScenarioContext _scenarioContext;
        private readonly MjolnirApiFixture _fixture;

        public WieldStepDefinitions(ScenarioContext scenarioContext, ITestOutputHelper outputHelper, MjolnirApiFixture fixture)
        {
            _scenarioContext = scenarioContext;
            _fixture = fixture;
            _fixture.XunitTestOutputHelper = outputHelper;
        }

        [Given(@"the hero (.*) has been created")]
        public void GivenTheWorthyHeroHasBeenCreated(string heroName)
        {
            _scenarioContext["heroName"] = heroName;
        }

        [Given(@"the hero is (.*)")]
        public void AndTheHeroIs(string worthiness)
        {
            _scenarioContext["worthiness"] = worthiness;
        }

        [Given(@"the hero has obtained their Asgard pass")]
        public void AndTheHeroHasObtainedAnAsgardPass()
        {
            var hero = (string)_scenarioContext["heroName"];
            var worthiness = (string)_scenarioContext["worthiness"];
            var bifrostOptions = _fixture
                                .Services
                                .GetRequiredService<IOptions<BifrostConfiguration>>()
                                .Value;

            // create bifrost pass (JWT) for hero with isworthy claim
            var jwt = GenerateJwt(hero, worthiness, bifrostOptions);

            // sign pass with secret
            _scenarioContext["jwt"] = jwt;
        }

        [When(@"the hero attempts to wield Mjolnir")]
        public async Task WhenTheHeroAttemptsToWeildMjolnir()
        {
            var jwt = (string)_scenarioContext["jwt"];

            using var client = _fixture.CreateClient();

            var request = new HttpRequestMessage(HttpMethod.Get, "mjolnir")
            {
                Headers =
                {
                    Authorization  = new AuthenticationHeaderValue("Bearer", jwt)
                }
            };

            var response = await client.SendAsync(request);

            _scenarioContext["response"] = response;
        }

        [Then(@"they should be successful and be deemed worthy")]
        public void ThenTheyShouldBeSuccessfulAndDeemedWorthy()
        {
            var response = _scenarioContext["response"] as HttpResponseMessage;
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.ReasonPhrase.Should().Be("Worthy");
        }

        [Then(@"they should be unsuccessful and be deemed unworthy")]
        public void ThenTheyShouldBeUnsuccessfulAndDeemedUnworthy()
        {
            var response = _scenarioContext["response"] as HttpResponseMessage;
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
            response.ReasonPhrase.Should().Be("Unworthy");
        }

        [Then(@"they should be unsuccessful and be banished from Asgard")]
        public void ThenTheyShouldBeUnsuccessfulAndBeBanishedFromAsgard()
        {
            var response = _scenarioContext["response"] as HttpResponseMessage;
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
            response.ReasonPhrase.Should().Be("Banished");
        }

        private static string GenerateJwt(string heroName, string worthiness, BifrostConfiguration bifrostOptions)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, heroName),
                new Claim(AsgardianClaims.Worthiness, worthiness)
            };

            var signingKey = new SymmetricSecurityKey(GetBytes(bifrostOptions.Secret));

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
