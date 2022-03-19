using System;
using Xunit;
using Unit.Utilities;
using Heimdall.Infrastructure;
using FluentAssertions;
using AutoFixture;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Heimdall.Exceptions;
using Unit.Doubles;

namespace Unit.Infrastructure
{
    public partial class HeroRegistrationServiceTests
    {
        public partial class TheRegisterAsyncMethod : UnitTest
        {
            [Fact]
            public async Task ShouldCreateHeroGivenNoHeroAlreadyExistsWithSameName()
            {
                var heroName = Fixture.Create<string>();
                var password = Fixture.Create<string>();
                var userManagerDouble = Fixture.Create<UserManagerCreateUserSuccessStub<IdentityUser>>();
                var sut = new HeroesManagerService(userManagerDouble);

                var result = await Record.ExceptionAsync(() => sut.RegisterHeroAync(heroName, password));

                result.Should().BeNull();
            }

            [Fact]
            public async Task ShouldThrowHeroNameTakenExceptionGivenHeroAlreadyExistsWithSameName()
            {
                var heroName = Fixture.Create<string>();
                var password = Fixture.Create<string>();
                var userManagerDouble = Fixture.Create<UserManagerPreExistingUserStub<IdentityUser>>();
                var sut = new HeroesManagerService(userManagerDouble);

                var result = await Record.ExceptionAsync(() => sut.RegisterHeroAync(heroName, password));

                result.Should().BeOfType<HeroNameTakenException>();
            }

            [Theory]
            [InlineData("", "")]
            [InlineData(" ", " ")]
            [InlineData(" ", "")]
            [InlineData("", " ")]
            [InlineData(null, null)]
            [InlineData(null, " ")]
            [InlineData(" ", null)]
            public async Task ShouldThrowArgumentNullExceptionGivenEmptyHeroNameOrPasswordOrBoth(string heroName, string passowrd)
            {
                var userManagerDouble = Fixture.Create<UserManager<IdentityUser>>();
                var sut = new HeroesManagerService(userManagerDouble);
                var result = await Record.ExceptionAsync(() => sut.RegisterHeroAync(heroName, passowrd));
                result.Should().BeOfType<ArgumentNullException>();
            }
        }
    }
}
