using FluentValidation;
using Microsoft.Extensions.Localization;
using SchoolProject.Core.Features.Authorization.Commands.Modles;
using SchoolProject.Core.ShResources;
using SchoolProject.Service.Abstracts;

namespace SchoolProject.Core.Features.Authorization.Commands.Validators
{
    public class AddRoleValidator : AbstractValidator<AddRoleCommand>
    {
        #region Fields
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        private readonly IAuthorizationService _authorizationsService;
        #endregion
        #region Constructor
        public AddRoleValidator(IStringLocalizer<SharedResources> stringLocalizer, IAuthorizationService authorizationsService)
        {
            _stringLocalizer = stringLocalizer;
            ApplyValidationsRules();
            ApplyCustomValidationsRules();
            _authorizationsService = authorizationsService;
        }
        #endregion
        #region actions
        public void ApplyValidationsRules()
        {
            RuleFor(x => x.RoleName)
                .NotEmpty().WithMessage(_stringLocalizer[SharedResourcesKeys.NotEmpty])
                .NotNull().WithMessage(_stringLocalizer[SharedResourcesKeys.NotNull]);

        }
        public void ApplyCustomValidationsRules()
        {
            RuleFor(x => x.RoleName)
                .MustAsync(async (Key, CancellationToken) => !await _authorizationsService.IsRoleExist(Key))
                .WithMessage(_stringLocalizer[SharedResourcesKeys.IsExist]);
        }
        #endregion
    }
}

