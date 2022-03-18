using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Heimdall.Controllers;
using Heimdall.Exceptions;
using Heimdall.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Unit.Utilities;
using Xunit;

namespace Unit.Controllers
{
    public class HeroesControllerTests
    {
        public class POST_RegisterHero : UnitTest
        {
            [Fact]
            public async Task ShouldReturn201CreatedStatucCodeGivenHeroNameAndPasswordAsync()
            {
                string name = Fixture.Create<string>();
                string password = Fixture.Create<string>();

                Fixture.Freeze<Mock<IHeroRegistrationService>>()
                .Setup(o => o.RegisterHeroAync(name, password))
                .Returns(Task.CompletedTask);

                var sut = Fixture.Build<HeroesController>()
                                 .OmitAutoProperties() //https://github.com/AutoFixture/AutoFixture/issues/1141
                                 .Create();

                var result = await sut.RegisterHero(name, password);

                result.Should().BeOfType<CreatedResult>()
                      .Which.StatusCode.Should().Be(201);
            }


            [Fact]
            public async Task ShouldReturn409ConflictStatusCodeGivenHeroAlreadyExistsWithSameNameAsync()
            {
                string name = Fixture.Create<string>();
                string password = Fixture.Create<string>();

                Fixture.Freeze<Mock<IHeroRegistrationService>>()
                .Setup(o => o.RegisterHeroAync(name, password))
                .Throws(new HeroNameTakenException(name));

                var sut = Fixture.Build<HeroesController>()
                                 .OmitAutoProperties() //https://github.com/AutoFixture/AutoFixture/issues/1141
                                 .Create();

                var result = await sut.RegisterHero(name, password);

                result.Should().BeOfType<ConflictObjectResult>()
                      .Which.StatusCode.Should().Be(409);
            }


            [Fact]
            public async Task ShouldReturnCorrectErrorMessageGivenHeroAlreadyExistsWithSameNameAsync()
            {
                string name = Fixture.Create<string>();
                string password = Fixture.Create<string>();

                Fixture.Freeze<Mock<IHeroRegistrationService>>()
                .Setup(o => o.RegisterHeroAync(name, password))
                .Throws(new HeroNameTakenException(name));

                var sut = Fixture.Build<HeroesController>()
                                 .OmitAutoProperties() //https://github.com/AutoFixture/AutoFixture/issues/1141
                                 .Create();

                var result = await sut.RegisterHero(name, password);

                result.Should().BeOfType<ConflictObjectResult>()
                      .Which.Value.Should().Be($"Hero name is already taken. {name}");
            }
        }
    }
}