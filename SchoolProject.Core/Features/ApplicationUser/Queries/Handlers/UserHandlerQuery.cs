using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using SchoolProject.Core.Bases;
using SchoolProject.Core.Features.ApplicationUser.Queries.Models;
using SchoolProject.Core.Features.ApplicationUser.Queries.Response;
using SchoolProject.Core.ShResources;
using SchoolProject.Core.Wrappers;
using SchoolProject.Data.Entities.Identity;

namespace SchoolProject.Core.Features.ApplicationUser.Queries.Handlers
{
    public class UserHandlerQuery : ResponseHandler
                                    , IRequestHandler<GetUserPaginationQuery, PaginatedResult<GetUserListResponse>>
                                    , IRequestHandler<GetUserByIdQuery, Response<GetUserByIdResponse>>
    {
        #region Fields    
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        #endregion
        #region Constructors
        public UserHandlerQuery(IStringLocalizer<SharedResources> stringLocalizer, IMapper mapper, UserManager<User> userManager) : base(stringLocalizer)
        {
            _stringLocalizer = stringLocalizer;
            _mapper = mapper;
            _userManager = userManager;
        }
        #endregion
        #region handle fuc
        public async Task<PaginatedResult<GetUserListResponse>> Handle(GetUserPaginationQuery request, CancellationToken cancellationToken)
        {
            var users = _userManager.Users.AsQueryable();
            var pginatedList = await _mapper.ProjectTo<GetUserListResponse>(users)
                                           .ToPaginatedListAsync(request.PageNumber, request.PageSize);
            return pginatedList;
        }

        public async Task<Response<GetUserByIdResponse>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            // var user = _userManager.FindByIdAsync(request.Id.ToString());
            var user1 = await _userManager.Users.FirstOrDefaultAsync(x => x.Id.Equals(request.Id));
            if (user1 == null) return NotFound<GetUserByIdResponse>(_stringLocalizer[SharedResourcesKeys.NotFoundId]);
            var mappe = _mapper.Map<GetUserByIdResponse>(user1);
            return Success(mappe);
        }
        #endregion
    }
}
