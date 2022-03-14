using Asgard.Infrastructure;
using AutoFixture;
using FluentAssertions;
using Unit.Utilities;
using Xunit;

namespace Unit.Infrastructure
{
    public class WorthyHerosListTests
    {
        public class TheContainsMethod : UnitTest
        {
            [Theory]
            [InlineData("Thor")]
            [InlineData("Captain America")]
            [InlineData("Black Panther")]
            [InlineData("Loki")]
            [InlineData("Vision")]
            [InlineData("Superman")]
            public void ShouldReturnTrueGivenMcuWorthyHeroName(string heroName)
            {
                var sut = Fixture.Create<WorthyHerosList>();

                var result = sut.Contains(heroName);

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
                var sut = Fixture.Create<WorthyHerosList>();

                var result = sut.Contains(heroName);

                result.Should().BeFalse();
            }
        }
    }
}