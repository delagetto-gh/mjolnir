using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace Unit.Doubles
{
    internal class UserManagerUserAlreadyExistsStub<TUser> : UserManager<TUser> where TUser : class, new()
    {
        public UserManagerUserAlreadyExistsStub(IUserStore<TUser> store, IOptions<IdentityOptions> optionsAccessor, IPasswordHasher<TUser> passwordHasher, IEnumerable<IUserValidator<TUser>> userValidators, IEnumerable<IPasswordValidator<TUser>> passwordValidators, ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, IServiceProvider services, ILogger<UserManager<TUser>> logger) : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
        }

        public override Task<TUser> FindByNameAsync(string userName)
        {
            return Task.FromResult(new TUser());
        }
    }
}
