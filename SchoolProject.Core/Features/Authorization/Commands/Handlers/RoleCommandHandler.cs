using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using SchoolProject.Core.Bases;
using SchoolProject.Core.Features.Authorization.Commands.Modles;
using SchoolProject.Core.ShResources;
using SchoolProject.Service.Abstracts;

namespace SchoolProject.Core.Features.Authorization.Commands.Handlers
{
    public class RoleCommandHandler : ResponseHandler
                                    , IRequestHandler<AddRoleCommand, Response<string>>,
                                      IRequestHandler<EditRoleCommand, Response<string>>,
                                      IRequestHandler<DeleteRoleCommand, Response<string>>,
                                      IRequestHandler<UpdateUserRolesCommand, Response<string>>
    {
        #region Fildes
        public readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        private readonly IAuthorizationService _authorizationService;
        #endregion
        #region constructor
        public RoleCommandHandler(IMapper mapper, IStringLocalizer<SharedResources> stringLocalizer, IAuthorizationService authorizationService) : base(stringLocalizer)
        {
            _mapper = mapper;
            _stringLocalizer = stringLocalizer;
            _authorizationService = authorizationService;
        }
        #endregion
        #region handelfuc
        public async Task<Response<string>> Handle(AddRoleCommand request, CancellationToken cancellationToken)
        {
            var result = await _authorizationService.AddRoleAsync(request.RoleName);
            if (result == "Success") return Success(result);
            return BadRequest<string>(_stringLocalizer[SharedResourcesKeys.AddFaild]);
        }

        public async Task<Response<string>> Handle(EditRoleCommand request, CancellationToken cancellationToken)
        {
            var result = await _authorizationService.EditRoleAsync(request.Id, request.Name);
            if (result == "notFound") return NotFound<string>();
            else if (result == "Success") return Success<string>(_stringLocalizer[SharedResourcesKeys.Updated]);
            else
                return BadRequest<string>(result);

        }

        public async Task<Response<string>> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
        {
            var result = await _authorizationService.DeletrRoleAsync(request.Id);
            if (result == "NotFound") return NotFound<string>();
            else if (result == "ThisRoleIsUsed") return BadRequest<string>(_stringLocalizer[SharedResourcesKeys.ThisRoleIsUsed]);
            else if (result == "Success") return Success<string>(_stringLocalizer[SharedResourcesKeys.Deleted]);
            else
                return BadRequest<string>(result);
        }

        public async Task<Response<string>> Handle(UpdateUserRolesCommand request, CancellationToken cancellationToken)
        {
            var result = await _authorizationService.UpdateUserRoles(request);
            switch (result)
            {
                case "UserIsNull": return NotFound<string>(_stringLocalizer[SharedResourcesKeys.NotFoundUserId]);
                case "FialedToRemoveOldRoles": return NotFound<string>(_stringLocalizer[SharedResourcesKeys.FialedToRemoveOldRoles]);
                case "FialedToAddNewRoles": return NotFound<string>(_stringLocalizer[SharedResourcesKeys.FialedToAddNewRoles]);
                case "Success": return NotFound<string>(_stringLocalizer[SharedResourcesKeys.Success]);
                case "FialedToUpdateUserRoles": return NotFound<string>(_stringLocalizer[SharedResourcesKeys.FialedToUpdateUserRoles]);
            }
            return Success<string>(_stringLocalizer[SharedResourcesKeys.Success]);
        }
        #endregion
    }
}
