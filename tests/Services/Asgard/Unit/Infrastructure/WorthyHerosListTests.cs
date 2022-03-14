using Asgard.Infrastructure;
using FluentAssertions;
using Moq.AutoMock;
using Xunit;

namespace Unit.Infrastructure
{
    public class WorthyHerosListTests
    {
        public class TheContainsMethod
        {
            private readonly WorthyHerosList _sut;
            private readonly AutoMocker _fixture = new AutoMocker();

            public TheContainsMethod()
            {
                _sut = _fixture.CreateInstance<WorthyHerosList>();
            }

            [Theory]
            [InlineData("Thor")]
            [InlineData("Captain America")]
            [InlineData("Black Panther")]
            [InlineData("Loki")]
            [InlineData("Vision")]
            [InlineData("Superman")]
            public void ShouldReturnTrueGivenMcuWorthyHeroName(string heroName)
            {
                var result = _sut.Contains(heroName);

                result.Should().BeTrue();
            }

            [Theory]
            [InlineData("Thot")]
            [InlineData("Captain Underpants")]
            [InlineData("Black Rabbit")]
            [InlineData("Four Loko")]
            [InlineData("Sight")]
            [InlineData("Soupman")]
            public void ShouldReturnFalseGivenNonMcuWorthyHeroName(string heroName)
            {
                var result = _sut.Contains(heroName);

                result.Should().BeFalse();
            }
        }
    }
}