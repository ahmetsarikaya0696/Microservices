using FreeCourse.IdentityServer4.Models;
using IdentityModel;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FreeCourse.IdentityServer4.Services
{
    public class IdentityResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public IdentityResourceOwnerPasswordValidator(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var user = await _userManager.FindByEmailAsync(context.UserName);
            var errors = new Dictionary<string, object>();

            if (user == null)
            {
                errors.Add("errors", new List<string>() { "Email veya şifreniz yanlış!" });
                context.Result.CustomResponse = errors;
                return;
            }

            bool passwordCheck = await _userManager.CheckPasswordAsync(user, context.Password);

            if (!passwordCheck)
            {
                errors.Add("errors", new List<string>() { "Email veya şifreniz yanlış!" });
                context.Result.CustomResponse = errors;
                return;
            }

            context.Result = new GrantValidationResult(user.Id.ToString(), OidcConstants.AuthenticationMethods.Password);
        }
    }
}
