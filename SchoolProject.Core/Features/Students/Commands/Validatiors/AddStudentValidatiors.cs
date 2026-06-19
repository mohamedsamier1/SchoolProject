using FluentValidation;
using Microsoft.Extensions.Localization;
using SchoolProject.Core.Features.Students.Commands.Models;
using SchoolProject.Core.ShResources;
using SchoolProject.Service.Abstracts;

namespace SchoolProject.Core.Features.Students.Commands.Validatiors
{
    public class AddStudentValidatiors : AbstractValidator<AddStudentCommand>
    {
        #region Fields
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        private readonly IDepartmentService _departmentService;

        #endregion
        #region constractors
        public AddStudentValidatiors(IStringLocalizer<SharedResources> stringLocalizer, IDepartmentService departmentService)
        {
            _stringLocalizer = stringLocalizer;
            ApplyValidationsRules();
            ApplyCustomValidationsRules();
            _departmentService = departmentService;
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
            RuleFor(x => x.DepartmentId)
                .NotEmpty().WithMessage(_stringLocalizer[SharedResourcesKeys.NotEmpty])
                .NotNull().WithMessage(_stringLocalizer[SharedResourcesKeys.Required]);



        }
        public void ApplyCustomValidationsRules()
        {
            RuleFor(x => x.DepartmentId)
                .MustAsync(async (Key, CancellationToken) => await _departmentService.IsDepartmentIdExist(Key))
                .WithMessage(_stringLocalizer[SharedResourcesKeys.NotFoundId]);
        }
        #endregion
    }
}
