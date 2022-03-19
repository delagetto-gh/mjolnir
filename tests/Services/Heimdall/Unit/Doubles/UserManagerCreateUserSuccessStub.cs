using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace Unit.Doubles
{
    internal class UserManagerCreateUserSuccessStub<TUser> : UserManager<TUser> where TUser : class
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
}

