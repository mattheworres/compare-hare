#region usings

using System.Threading.Tasks;
using CompareHare.Api.Features.Authentication.Models;
using CompareHare.Api.Features.Shared.Validation;
using CompareHare.Api.Features.Shared.Validation.Interfaces;
using CompareHare.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

#endregion

namespace CompareHare.Api.Features.Authentication.RequestHandlers.LogIn
{
    public class LogInModelCustomValidator : ICustomValidator<LogInModel>
    {
        private const int MAXIMUM_LOGIN_ATTEMPTS = 5;
        private readonly CompareHareDbContext _dbContext;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public LogInModelCustomValidator(
            CompareHareDbContext dbContext,
            UserManager<User> userManager,
            SignInManager<User> signInManager
            )
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<CustomValidationFailures> ValidateAsync(LogInModel model)
        {
            var customErrors = new CustomValidationFailures();

            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == model.Email);

            if (user == null) return LoginFailure(customErrors);

            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, lockoutOnFailure: false);

            // if (result.Succeeded) {
            if (await _userManager.IsEmailConfirmedAsync(user) == false) return UnconfirmedFailure(customErrors);
            await _userManager.ResetAccessFailedCountAsync(user);
            // } else {
            // await _userManager.AccessFailedAsync(user);
            //     if (result.IsLockedOut || user.AccessFailedCount >= MAXIMUM_LOGIN_ATTEMPTS) return TooManyAttemptsFailure(customErrors);
            //     return LoginFailure(customErrors);
            // }

            return customErrors;
        }

        private static CustomValidationFailures LoginFailure(CustomValidationFailures customErrors)
        {
            customErrors.Add("email", "InvalidCredentials");
            customErrors.Add("password", "");

            return customErrors;
        }

        private static CustomValidationFailures TooManyAttemptsFailure(CustomValidationFailures customErrors)
        {
            customErrors.Add("email", "TooManyLoginAttempts");

            return customErrors;
        }

        private static CustomValidationFailures UnconfirmedFailure(CustomValidationFailures customErrors)
        {
            customErrors.Add("email", "UnconfirmedAccountOnLogin");

            return customErrors;
        }
    }
}
