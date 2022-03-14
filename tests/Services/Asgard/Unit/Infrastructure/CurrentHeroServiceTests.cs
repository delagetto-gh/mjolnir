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
            [InlineData("GenericHeroName")]
            [InlineData("Captain Marvel")]
            [InlineData("Beowolf")]
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
                          var claims = new Claim[]
                          {
                                new Claim(ClaimTypes.Name, heroName)
                          };
                          var identity = new ClaimsIdentity(claims);
                          var user = new ClaimsPrincipal(identity);
                          return new DefaultHttpContext()
                          {
                              User = user
                          };
                      });
            }
        }
    }
}