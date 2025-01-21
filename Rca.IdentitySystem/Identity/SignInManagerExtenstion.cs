using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Rca.IdentitySystem.Models;

namespace Rca.IdentitySystem.Identity
{
    public class SignInManagerExtenstion<TUser> : SignInManager<TUser> where TUser : class
    {
        public SignInManagerExtenstion(UserManager<TUser> userManager, IHttpContextAccessor contextAccessor, IUserClaimsPrincipalFactory<TUser> claimsFactory, IOptions<IdentityOptions> optionsAccessor, ILogger<SignInManager<TUser>> logger, IAuthenticationSchemeProvider schemes, IUserConfirmation<TUser> confirmation) : base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemes, confirmation)
        {

        }

        public override async Task<SignInResult> PasswordSignInAsync(string userName, string password,
        bool isPersistent, bool lockoutOnFailure)
        {
            var user = await UserManager.FindByNameAsync(userName);
            if (user == null)
            {
                return SignInResult.Failed;
            }

            return await PasswordSignInAsync(user, password, isPersistent, lockoutOnFailure);
        }


    }
}
