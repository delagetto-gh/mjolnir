using System.Security.Claims;
using Asgard.Infrastructure;
using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using Unit.Utilities;
using Xunit;

namespace Unit.Infrastructure
{
    public class CurrentHeroServiceTests
    {
        public class TheGetMethod : UnitTest
        {
            [Fact]
            public void ShouldReturnHeroGivenHttpContextUserWithNameClaimType()
            {
                SetupHttpContextUser("someHeroName");

                var sut = Fixture.Create<CurrentHeroService>();

                var result = sut.Get();

                result.Should().NotBeNull();
            }

            [Theory]
            [InlineData("Thor")]
            [InlineData("Captain America")]
            [InlineData("Black Panther")]
            [InlineData("Loki")]
            [InlineData("Vision")]
            [InlineData("Superman")]
            [InlineData("Thot")]
            [InlineData("Captain Underpants")]
            [InlineData("Black Rabbit")]
            [InlineData("Four Loko")]
            [InlineData("Sight")]
            [InlineData("Soupman")]
            public void ShouldReturnCorrectHeroNameGivenHttpContextUserWithNameClaimType(string heroName)
            {
                SetupHttpContextUser(heroName);

                var sut = Fixture.Create<CurrentHeroService>();

                var result = sut.Get();

                result.Name.Should().Be(heroName);
            }

            private void SetupHttpContextUser(string heroName)
            {
                Fixture.Freeze<Mock<IHttpContextAccessor>>()
                        .Setup(o => o.HttpContext)
                        .Returns(() =>
                        {
                            var nameClaim = new Claim(ClaimTypes.Name, heroName);
                            var claims = new Claim[] { nameClaim };
                            var identity = new ClaimsIdentity(claims);
                            var user = new ClaimsPrincipal(identity);
                            var httpContext = new DefaultHttpContext { User = user };
                            return httpContext;
                        });
            }
        }
    }
}