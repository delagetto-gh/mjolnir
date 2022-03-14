using Asgard.Infrastructure;
using Asgard.Models;
using Asgard.Services;
using FluentAssertions;
using Moq.AutoMock;
using Xunit;

namespace Unit.Infrastructure
{
    public class MjolnirWieldingServiceTests
    {
        public class TheWieldMethod
        {
            private readonly MjolnirWieldingService _sut;
            private readonly AutoMocker _fixture = new AutoMocker();

            public TheWieldMethod()
            {
                _sut = _fixture.CreateInstance<MjolnirWieldingService>();
            }

            [Fact]
            public void ShouldReturnTrueGivenHeroNameIsWorthy()
            {
                var hero = new Hero("hero x");

                _fixture.GetMock<IWorthyHerosList>()
                .Setup(o => o.Contains(hero.Name))
                .Returns(true);

                var result = _sut.Wield(hero);

                result.Should().BeTrue();
            }

            [Fact]
            public void ShouldReturnFalseGivenHeroNameIsUnworthy()
            {
                var hero = new Hero("hero x");

                _fixture.GetMock<IWorthyHerosList>()
                .Setup(o => o.Contains(hero.Name))
                .Returns(false);

                var result = _sut.Wield(hero);

                result.Should().BeFalse();
            }
        }
    }
}