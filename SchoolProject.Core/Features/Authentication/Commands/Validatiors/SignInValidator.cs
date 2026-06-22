using FluentValidation;
using Microsoft.Extensions.Localization;
using SchoolProject.Core.Features.Authentication.Commands.Models;
using SchoolProject.Core.ShResources;

namespace SchoolProject.Core.Features.Authentication.Commands.Validatiors
{
    public class SignInValidator : AbstractValidator<SignInCommand>
    {
        #region Fields
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        #endregion
        #region constractors
        public SignInValidator(IStringLocalizer<SharedResources> stringLocalizer)
        {
            _stringLocalizer = stringLocalizer;
            ApplyValidationsRules();
            ApplyCustomValidationsRules();

        }
        #endregion
        #region actions
        public void ApplyValidationsRules()
        {
            RuleFor(x => x.UserName)
              .NotEmpty().WithMessage(_stringLocalizer[SharedResourcesKeys.NotEmpty])
              .NotNull().WithMessage(_stringLocalizer[SharedResourcesKeys.NotNull])
              .MaximumLength(100);
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage(_stringLocalizer[SharedResourcesKeys.NotEmpty])
                .NotNull().WithMessage(_stringLocalizer[SharedResourcesKeys.NotNull])
                .MaximumLength(100);

        }
        public void ApplyCustomValidationsRules()
        {

        }
        #endregion
    }
}
