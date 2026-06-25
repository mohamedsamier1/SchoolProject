using FluentValidation;
using Microsoft.Extensions.Localization;
using SchoolProject.Core.Features.Authorization.Commands.Modles;
using SchoolProject.Core.ShResources;
using SchoolProject.Service.Abstracts;

namespace SchoolProject.Core.Features.Authorization.Commands.Validators
{
    public class DeleteRoleValidator : AbstractValidator<DeleteRoleCommand>
    {
        #region Fields
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        private readonly IAuthorizationService _authorizationService;
        #endregion
        #region Constructor
        public DeleteRoleValidator(IStringLocalizer<SharedResources> stringLocalizer, IAuthorizationService authorizationService)
        {
            _stringLocalizer = stringLocalizer;
            ApplyValidationsRules();
            ApplyCustomValidationsRules();
            _authorizationService = authorizationService;
        }
        #endregion
        #region actions
        public void ApplyValidationsRules()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage(_stringLocalizer[SharedResourcesKeys.NotEmpty])
                .NotNull().WithMessage(_stringLocalizer[SharedResourcesKeys.NotNull]);


        }
        public void ApplyCustomValidationsRules()
        {
            //RuleFor(x => x.Id)
            //    .MustAsync(async (Key, CancellationToken) => !await _authorizationService.IsRoleExistById(Key))
            //    .WithMessage(_stringLocalizer[SharedResourcesKeys.IsExist]);          
        }
        #endregion
    }
}