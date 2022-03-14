using Asgard.Infrastructure;
using Asgard.Models;
using Asgard.Services;
using AutoFixture;
using FluentAssertions;
using Moq;
using Unit.Utilities;
using Xunit;

namespace Unit.Infrastructure
{
    public class MjolnirWieldingServiceTests
    {
        public class TheWieldMethod : UnitTest
        {
            [Fact]
            public void ShouldReturnTrueGivenHeroNameIsContainedInWorthyHeroList()
            {
                var hero = CreateHero();

                Fixture.Freeze<Mock<IWorthyHerosList>>()
                .Setup(o => o.Contains(hero.Name))
                .Returns(true);

                var sut = Fixture.Create<MjolnirWieldingService>();

                var result = sut.Wield(hero);

                result.Should().BeTrue();
            }


            [Fact]
            public void ShouldReturnFalseGivenHeroNameIsNotContainedInWorthyHeroList()
            {
                var hero = CreateHero();

                Fixture.Freeze<Mock<IWorthyHerosList>>()
                .Setup(o => o.Contains(hero.Name))
                .Returns(false);

                var sut = Fixture.Create<MjolnirWieldingService>();

                var result = sut.Wield(hero);

                result.Should().BeFalse();
            }

            private Hero CreateHero()
            {
                var heroName = Fixture.Create<string>();
                return new Hero(heroName);
            }
        }
    }
}