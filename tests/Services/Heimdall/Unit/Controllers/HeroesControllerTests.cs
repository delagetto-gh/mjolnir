using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Heimdall.Controllers;
using Heimdall.Exceptions;
using Heimdall.Requests;
using Heimdall.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Unit.Utilities;
using Xunit;

namespace Unit.Controllers
{
    public class HeroesControllerTests
    {
        public class The_POST_RegisterHeroMethod : UnitTest
        {
            [Fact]
            public async Task ShouldReturn201CreatedStatucCodeGivenHeroNameAndPasswordAsync()
            {
                var request = Fixture.Create<RegisterHeroRequest>();

                Fixture.Freeze<Mock<IHeroRegistrationService>>()
                .Setup(o => o.RegisterHeroAync(request.HeroName, request.Password))
                .Returns(Task.CompletedTask);

                var sut = Fixture.Build<HeroesController>()
                                 .OmitAutoProperties() //https://github.com/AutoFixture/AutoFixture/issues/1141
                                 .Create();

                var result = await sut.RegisterHero(request);

                result.Should().BeOfType<CreatedResult>()
                      .Which.StatusCode.Should().Be(201);
            }


            [Fact]
            public async Task ShouldReturn409ConflictStatusCodeGivenHeroAlreadyExistsWithSameNameAsync()
            {
                var request = Fixture.Create<RegisterHeroRequest>();

                Fixture.Freeze<Mock<IHeroRegistrationService>>()
                .Setup(o => o.RegisterHeroAync(request.HeroName, request.Password))
                .Throws(new HeroNameTakenException(request.HeroName));

                var sut = Fixture.Build<HeroesController>()
                                 .OmitAutoProperties() //https://github.com/AutoFixture/AutoFixture/issues/1141
                                 .Create();

                var result = await sut.RegisterHero(request);

                result.Should().BeOfType<ConflictObjectResult>()
                      .Which.StatusCode.Should().Be(409);
            }


            [Fact]
            public async Task ShouldReturnCorrectErrorMessageGivenHeroAlreadyExistsWithSameNameAsync()
            {
                var request = Fixture.Create<RegisterHeroRequest>();

                Fixture.Freeze<Mock<IHeroRegistrationService>>()
                .Setup(o => o.RegisterHeroAync(request.HeroName, request.Password))
                .Throws(new HeroNameTakenException(request.HeroName));

                var sut = Fixture.Build<HeroesController>()
                                 .OmitAutoProperties() //https://github.com/AutoFixture/AutoFixture/issues/1141
                                 .Create();

                var result = await sut.RegisterHero(request);

                result.Should().BeOfType<ConflictObjectResult>()
                      .Which.Value.Should().Be($"Hero name is already taken. {request.HeroName}");
            }
        }
    }
}