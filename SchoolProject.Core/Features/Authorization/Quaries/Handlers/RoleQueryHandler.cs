using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using SchoolProject.Core.Bases;
using SchoolProject.Core.Features.Authorization.Quaries.Models;
using SchoolProject.Core.Features.Authorization.Quaries.Result;
using SchoolProject.Core.ShResources;
using SchoolProject.Data.DTOS;
using SchoolProject.Data.Entities.Identity;
using SchoolProject.Service.Abstracts;

namespace SchoolProject.Core.Features.Authorization.Quaries.Handlers
{
    public class RoleQueryHandler : ResponseHandler,
                                    IRequestHandler<GetRoleListQuery, Response<List<GetRoleResponse>>>,
                                    IRequestHandler<GetRoleByIdQuery, Response<GetRoleByIdResponse>>,
                                    IRequestHandler<ManageUserRolesQuery, Response<ManageUserRolesDto>>

    {
        #region Fildes
        public readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        private readonly IAuthorizationService _authorizationService;
        private readonly UserManager<User> _userManager;
        #endregion
        #region constructor
        public RoleQueryHandler(IMapper mapper, IStringLocalizer<SharedResources> stringLocalizer, IAuthorizationService authorizationService, UserManager<User> userManager) : base(stringLocalizer)
        {
            _mapper = mapper;
            _stringLocalizer = stringLocalizer;
            _authorizationService = authorizationService;
            _userManager = userManager;
        }
        #endregion
        #region handelfuc
        public async Task<Response<List<GetRoleResponse>>> Handle(GetRoleListQuery request, CancellationToken cancellationToken)
        {
            var roles = await _authorizationService.GetRoleList();
            var mapper = _mapper.Map<List<GetRoleResponse>>(roles);
            return Success(mapper);
        }
        async Task<Response<GetRoleByIdResponse>> IRequestHandler<GetRoleByIdQuery, Response<GetRoleByIdResponse>>.Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
        {
            var role = await _authorizationService.GetRoleById(request.Id);
            if (role == null) return NotFound<GetRoleByIdResponse>(_stringLocalizer[SharedResourcesKeys.NotFoundId]);
            var mapper = _mapper.Map<GetRoleByIdResponse>(role);
            return Success(mapper);
        }

        public async Task<Response<ManageUserRolesDto>> Handle(ManageUserRolesQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId.ToString());
            if (user == null) return NotFound<ManageUserRolesDto>(_stringLocalizer[SharedResourcesKeys.NotFoundUserId]);
            var result = await _authorizationService.GetUserIdAndRole(user);
            return Success(result);
        }
        #endregion

    }
}
