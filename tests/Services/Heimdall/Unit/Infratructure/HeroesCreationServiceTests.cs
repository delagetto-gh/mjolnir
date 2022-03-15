using System;
using Xunit;
using Unit.Utilities;
using Heimdall.Infrastructure;
using FluentAssertions;
using AutoFixture;

namespace Unit.Infratructure
{
    public class HeroesCreationServiceTests
    {
        public class TheCreateHeroMethod : UnitTest
        {
            [Fact]
            public void ShouldReturnHeroInstanceGivenNoHeroAlreadyExistsWithSameName()
            {
                var heroName = Fixture.Create<string>();
                var password = Fixture.Create<string>();
                var sut = new HeroesCreationService();

                var result = sut.CreateHero(heroName, password);

                result.Should().NotBeNull();
            }

            [Fact]
            public void ShouldReturnCorrectHeroNameGivenNoHeroAlreadyExistsWithSameName()
            {
                var heroName = Fixture.Create<string>();
                var password = Fixture.Create<string>();
                var sut = new HeroesCreationService();

                var result = sut.CreateHero(heroName, password);

                result.Name.Should().Be(heroName);
            }

            [Theory]
            [InlineData("", "")]
            [InlineData(" ", " ")]
            [InlineData(" ", "")]
            [InlineData("", " ")]
            [InlineData(null, null)]
            [InlineData(null, " ")]
            [InlineData(" ", null)]
            public void ShouldThrowArgumentNullExceptionGivenEmptyHeroNameOrPasswordOrBoth(string heroName, string passowrd)
            {
                var sut = new HeroesCreationService();
                var result = Record.Exception(() => sut.CreateHero(heroName, passowrd));
                result.Should().BeOfType<ArgumentNullException>();
            }
        }
    }
}
