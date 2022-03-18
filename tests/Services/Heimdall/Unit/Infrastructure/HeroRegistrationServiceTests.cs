using System;
using Xunit;
using Unit.Utilities;
using Heimdall.Infrastructure;
using FluentAssertions;
using AutoFixture;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Moq;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Heimdall.Exceptions;

namespace Unit.Infrastructure
{
    public class HeroRegistrationServiceTests
    {
        public class TheRegisterAsyncMethod : UnitTest
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
                var userManagerDouble = Fixture.Create<UserManagerNameAlreadyExistsStub<IdentityUser>>();
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
                var userManagerDouble = Fixture.Create<UserManagerCreateUserSuccessStub<IdentityUser>>();
                var sut = new HeroesManagerService(userManagerDouble);
                var result = await Record.ExceptionAsync(() => sut.RegisterHeroAync(heroName, passowrd));
                result.Should().BeOfType<ArgumentNullException>();
            }

            private class UserManagerCreateUserSuccessStub<TUser> : UserManager<TUser> where TUser : class
            {
                public UserManagerCreateUserSuccessStub(IUserStore<TUser> store, IOptions<IdentityOptions> optionsAccessor, IPasswordHasher<TUser> passwordHasher, IEnumerable<IUserValidator<TUser>> userValidators, IEnumerable<IPasswordValidator<TUser>> passwordValidators, ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, IServiceProvider services, ILogger<UserManager<TUser>> logger) : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
                {
                }

                public override Task<TUser> FindByNameAsync(string userName)
                {
                    return Task.FromResult(default(TUser));
                }

                public override Task<IdentityResult> CreateAsync(TUser user, string password)
                {
                    return Task.FromResult(IdentityResult.Success);
                }
            }

            private class UserManagerNameAlreadyExistsStub<TUser> : UserManager<TUser> where TUser : class, new()
            {
                public UserManagerNameAlreadyExistsStub(IUserStore<TUser> store, IOptions<IdentityOptions> optionsAccessor, IPasswordHasher<TUser> passwordHasher, IEnumerable<IUserValidator<TUser>> userValidators, IEnumerable<IPasswordValidator<TUser>> passwordValidators, ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, IServiceProvider services, ILogger<UserManager<TUser>> logger) : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
                {
                }

                public override Task<TUser> FindByNameAsync(string userName)
                {
                    return Task.FromResult(new TUser());
                }
            }
        }
    }
}
