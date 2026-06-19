using FluentValidation;
using Microsoft.Extensions.Localization;
using SchoolProject.Core.Features.Students.Commands.Models;
using SchoolProject.Core.ShResources;

namespace SchoolProject.Core.Features.Students.Commands.Validatiors
{
    public class AddStudentValidatiors : AbstractValidator<AddStudentCommand>
    {
        #region Fields
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;

        #endregion
        #region constractors
        public AddStudentValidatiors(IStringLocalizer<SharedResources> stringLocalizer)
        {
            _stringLocalizer = stringLocalizer;
            ApplyValidationsRules();
            ApplyCustomValidationsRules();

        }
        #endregion
        #region actions
        public void ApplyValidationsRules()
        {
            RuleFor(x => x.NameAr)
                .NotEmpty()
                .NotNull()
                .MaximumLength(50);
            RuleFor(x => x.NameEn)
                .NotEmpty()
                .NotNull()
                .MaximumLength(50);
            RuleFor(x => x.Address)
                .NotEmpty().WithMessage(_stringLocalizer[SharedResourcesKeys.NotEmpty])
                .NotNull()
                .MaximumLength(50);

        }
        public void ApplyCustomValidationsRules()
        {

        }
        #endregion
    }
}
