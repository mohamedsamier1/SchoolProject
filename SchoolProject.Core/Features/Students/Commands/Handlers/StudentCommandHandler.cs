using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using SchoolProject.Core.Bases;
using SchoolProject.Core.Features.Students.Commands.Models;
using SchoolProject.Core.ShResources;
using SchoolProject.Data.Entities;
using SchoolProject.Service.Abstracts;

namespace SchoolProject.Core.Features.Students.Commands.Handlers
{
    public class StudentCommandHandler : ResponseHandler, IRequestHandler<AddStudentCommand, Response<string>>,
                                                          IRequestHandler<EditStudentCommand, Response<string>>,
                                                          IRequestHandler<DeleteStudentCommand, Response<string>>
    {
        #region Fildes
        private readonly IStudentService _studentService;
        public readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        #endregion
        #region Constructor
        public StudentCommandHandler(IStudentService studentService, IMapper mapper, IStringLocalizer<SharedResources> stringLocalizer) : base(stringLocalizer)
        {
            _studentService = studentService;
            _mapper = mapper;
            _stringLocalizer = stringLocalizer;
        }
        #endregion
        #region HandelFunction
        public async Task<Response<string>> Handle(AddStudentCommand request, CancellationToken cancellationToken)
        {
            //Make Mapping Between raquest and student
            var studentmapper = _mapper.Map<Student>(request);
            //add
            var result = await _studentService.CreatNewStudentAsync(studentmapper);
            //check condition 
            if (result == "Exist")
                return UnprocessableEntity<string>("Name is Exist");
            else if (result == "Invalid Department Id")
                return BadRequest<string>("Invalid Department Id");
            else if (result == "Success")
                return Created<string>(_stringLocalizer[SharedResourcesKeys.Created]);
            else return BadRequest<string>();
            //return response
        }

        public async Task<Response<string>> Handle(EditStudentCommand request, CancellationToken cancellationToken)
        {
            var studentid = await _studentService.GetStudentByIdAsync(request.Id);
            if (studentid == null) return NotFound<string>("Not Found Id");
            var studentmapper = _mapper.Map<Student>(request);
            var result = await _studentService.EditStudentAsync(studentmapper);
            if (result == "Success")
                return Success($"{_stringLocalizer[SharedResourcesKeys.Success]}{studentmapper.Id}");
            else if (result == "Invalid Department Id")
                return BadRequest<string>("Invalid Department Id");
            else return BadRequest<string>();

        }

        public async Task<Response<string>> Handle(DeleteStudentCommand request, CancellationToken cancellationToken)
        {
            var studentid = await _studentService.GetStudentByIdAsync(request.Id);
            if (studentid == null) return NotFound<string>("Not Found Id");
            var result = await _studentService.DeletStudentAsyAsync(studentid);
            if (result == "Deleted Success")
            {
                return Deleted<string>($"{_stringLocalizer[SharedResourcesKeys.Deleted]}:{request.Id}");
            }
            else return BadRequest<string>();
        }
        #endregion

    }
}
