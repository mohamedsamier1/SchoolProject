using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using SchoolProject.Core.Bases;
using SchoolProject.Core.Features.Authorization.Commands.Modles;
using SchoolProject.Core.ShResources;
using SchoolProject.Service.Abstracts;

namespace SchoolProject.Core.Features.Authorization.Commands.Handlers
{
    public class ClaimsCommandHandler : ResponseHandler,
                                      IRequestHandler<UpdateUserClaimsCommand, Response<string>>
    {
        #region Fildes
        public readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        private readonly IAuthorizationService _authorizationService;
        #endregion
        #region constructor
        public ClaimsCommandHandler(IMapper mapper, IStringLocalizer<SharedResources> stringLocalizer, IAuthorizationService authorizationService) : base(stringLocalizer)
        {
            _mapper = mapper;
            _stringLocalizer = stringLocalizer;
            _authorizationService = authorizationService;
        }
        #endregion
        #region MyRegion
        public async Task<Response<string>> Handle(UpdateUserClaimsCommand request, CancellationToken cancellationToken)
        {
            var result = await _authorizationService.UpdateUserClaims(request);
            switch (result)
            {
                case "UserIsNull": return NotFound<string>(_stringLocalizer[SharedResourcesKeys.NotFoundUserId]);
                case "FialedToRemoveOldClaim": return NotFound<string>(_stringLocalizer[SharedResourcesKeys.FialedToRemoveOldClaim]);
                case "FialedToAddNewClaim": return NotFound<string>(_stringLocalizer[SharedResourcesKeys.FialedToAddNewClaim]);
                case "Success": return NotFound<string>(_stringLocalizer[SharedResourcesKeys.Success]);
                case "FialedToUpdateUserClaim": return NotFound<string>(_stringLocalizer[SharedResourcesKeys.FialedToUpdateUserClaim]);
            }
            return Success<string>(_stringLocalizer[SharedResourcesKeys.Success]);
        }
        #endregion
    }
}
