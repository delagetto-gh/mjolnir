using System.Security.Claims;
using Asgard.Infrastructure;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq.AutoMock;
using Xunit;

namespace Unit.Infrastructure
{
    public class CurrentHeroServiceTests
    {
        public class TheGetMethod
        {
            private readonly CurrentHeroService _sut;
            private readonly AutoMocker _fixture = new AutoMocker();

            public TheGetMethod()
            {
                _sut = _fixture.CreateInstance<CurrentHeroService>();
            }

            [Fact]
            public void ShouldReturnHeroGivenHttpContextUserWithNameClaimType()
            {
                SetupHttpContextUser("someHeroName");

                var result = _sut.Get();

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

                var result = _sut.Get();

                result.Name.Should().Be(heroName);
            }

            private void SetupHttpContextUser(string heroName)
            {
                _fixture.GetMock<IHttpContextAccessor>()
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