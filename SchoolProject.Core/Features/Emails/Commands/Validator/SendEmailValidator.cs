using FluentValidation;
using Microsoft.Extensions.Localization;
using SchoolProject.Core.Features.Emails.Commands.Models;
using SchoolProject.Core.ShResources;

namespace SchoolProject.Core.Features.Emails.Commands.Validator
{
    public class SendEmailValidator : AbstractValidator<SendEmailCommand>
    {
        #region Fields
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;


        #endregion
        #region constractors
        public SendEmailValidator(IStringLocalizer<SharedResources> stringLocalizer)
        {
            _stringLocalizer = stringLocalizer;
            ApplyValidationsRules();
            ApplyCustomValidationsRules();

        }
        #endregion
        #region actions
        public void ApplyValidationsRules()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage(_stringLocalizer[SharedResourcesKeys.EmailMustNotBeEmpty])
                .NotNull().WithMessage(_stringLocalizer[SharedResourcesKeys.EmailMustNotBeNull])
                .MaximumLength(50);
            RuleFor(x => x.Message)
                .NotEmpty()
                .NotNull()
                .MaximumLength(50);




        }
        public void ApplyCustomValidationsRules()
        {
            //RuleFor(x => x.DepartmentId)
            //    .MustAsync(async (Key, CancellationToken) => await _departmentService.IsDepartmentIdExist(Key))
            //    .WithMessage(_stringLocalizer[SharedResourcesKeys.NotFoundId]);
        }
        #endregion
    }
}
