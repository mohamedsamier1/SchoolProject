using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using SchoolProject.Core.Bases;
using SchoolProject.Core.Features.Students.Queries.Models;
using SchoolProject.Core.Features.Students.Queries.Results;
using SchoolProject.Core.ShResources;
using SchoolProject.Core.Wrappers;
using SchoolProject.Data.Entities;
using SchoolProject.Service.Abstracts;
using System.Linq.Expressions;

namespace SchoolProject.Core.Features.Students.Queries.Handlers
{
    public class StudentQueryHandlerQuery : ResponseHandler,
                                                           IRequestHandler<GetStudentListQuery, Response<List<GetStudentListResponse>>>,
                                                           IRequestHandler<GetStudentByIdQuery, Response<GetStudentByIdResponse>>,
                                                           IRequestHandler<GetStudentPaginatedListQuery, PaginatedResult<GetStudentPaginatedListResponse>>
    {
        #region Fields
        private readonly IStudentService _studentService;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        #endregion
        #region Constructor
        public StudentQueryHandlerQuery(IStudentService studentService, IMapper mapper, IStringLocalizer<SharedResources> stringLocalizer) : base(stringLocalizer)
        {
            _studentService = studentService;
            _mapper = mapper;
            _stringLocalizer = stringLocalizer;
        }
        #endregion
        #region Handel Functions
        public async Task<Response<List<GetStudentListResponse>>> Handle(GetStudentListQuery request, CancellationToken cancellationToken)
        {
            var studentlist = await _studentService.GetStudentListAsync();
            var studentListMapper = _mapper.Map<List<GetStudentListResponse>>(studentlist);
            var result = Success(studentListMapper);
            result.Meta = new { Count = studentListMapper.Count() };
            return result;
        }

        public async Task<Response<GetStudentByIdResponse>> Handle(GetStudentByIdQuery request, CancellationToken cancellationToken)
        {
            var student = await _studentService.GetStudentByIdAsync(request.Id);
            if (student == null)
            {
                return NotFound<GetStudentByIdResponse>(_stringLocalizer[SharedResourcesKeys.NotFoundId]);
            }
            var result = _mapper.Map<GetStudentByIdResponse>(student);
            return Success(result);
        }

        public async Task<PaginatedResult<GetStudentPaginatedListResponse>> Handle(GetStudentPaginatedListQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<Student, GetStudentPaginatedListResponse>> expression = e => new GetStudentPaginatedListResponse(e.Id, e.GenerallLocalizedName(e.NameAr, e.NameEn), e.Address, e.GenerallLocalizedName(e.Department.DNameAr, e.Department.DNameEn));
            var FilterQuery = _studentService.FilterStuedntPaginatedQuerable(request.OrderBy, request.Search);
            var paginatedlist = await FilterQuery.Select(expression).ToPaginatedListAsync(request.PageNumber, request.PageSize);
            var result = paginatedlist;
            result.Meta = new { Count = paginatedlist.Data.Count() };
            return result;
        }
        #endregion

    }
}
