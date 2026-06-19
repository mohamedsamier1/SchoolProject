using FluentValidation;
using SchoolProject.Core.Features.Students.Commands.Models;

namespace SchoolProject.Core.Features.Students.Commands.Validatiors
{
    public class EditStudentValidator : AbstractValidator<EditStudentCommand>
    {
        #region Fields

        #endregion
        #region constractors
        public EditStudentValidator()
        {
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
                .NotEmpty().WithMessage("Address Must Not Be Empty")
                .NotNull()
                .MaximumLength(50);

        }
        public void ApplyCustomValidationsRules()
        {

        }
        #endregion
    }
}
