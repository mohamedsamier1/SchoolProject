using AutoMapper;

using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using SchoolProject.Core.Bases;
using SchoolProject.Core.Features.ApplicationUser.Commands.Models;
using SchoolProject.Core.ShResources;
using SchoolProject.Data.Entities.Identity;
using SchoolProject.Service.Abstracts;

namespace SchoolProject.Core.Features.ApplicationUser.Commands.Handlers
{
    public class UserHandlerCommand : ResponseHandler,
                                                    IRequestHandler<AddUserCommand, Response<string>>,
                                                    IRequestHandler<UpdateUserCommand, Response<string>>,
                                                    IRequestHandler<DeleteUserCommand, Response<string>>,
                                                    IRequestHandler<ChangeUserPasswordCommand, Response<string>>

    {
        #region filde
        public readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEmailService _emailService;
        private readonly IApplicationUserService _applicationUserService;

        #endregion

        #region Constractor
        public UserHandlerCommand(IMapper mapper, IStringLocalizer<SharedResources> stringLocalizer, UserManager<User> userManager, IHttpContextAccessor httpContextAccessor, IEmailService emailService, IApplicationUserService applicationUserService) : base(stringLocalizer)
        {
            _mapper = mapper;
            _stringLocalizer = stringLocalizer;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _emailService = emailService;
            _applicationUserService = applicationUserService;
        }
        #endregion
        #region Handelfunc
        public async Task<Response<string>> Handle(AddUserCommand request, CancellationToken cancellationToken)
        {

            var mapper = _mapper.Map<User>(request);
            var createreult = await _applicationUserService.AddUserAsync(mapper, request.Password);
            switch (createreult)
            {
                case "EmaliIsExist": return BadRequest<string>(_stringLocalizer[SharedResourcesKeys.EmaliIsExist]);
                case "UserNameIsExist": return BadRequest<string>(_stringLocalizer[SharedResourcesKeys.UserNameIsExist]);
                case "Failed": return BadRequest<string>(_stringLocalizer[SharedResourcesKeys.TryToRegisterAgain]);
                case "Success": return Success<string>("");
                default: return BadRequest<string>(createreult);
            }


        }

        public async Task<Response<string>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.Id.ToString());
            if (user == null) return BadRequest<string>(_stringLocalizer[SharedResourcesKeys.NotFoundId]);
            var mapper = _mapper.Map(request, user);
            var resutl = await _userManager.UpdateAsync(mapper);
            if (!resutl.Succeeded) return BadRequest<string>(
               string.Join(" | ",
               resutl.Errors.Select(e => e.Description)));
            return Success("");
        }

        public async Task<Response<string>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.Id.ToString());
            if (user == null) return BadRequest<string>(_stringLocalizer[SharedResourcesKeys.NotFoundId]);
            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded) return BadRequest<string>(
              string.Join(" | ",
              result.Errors.Select(e => e.Description)));
            return Deleted<string>();

        }

        public async Task<Response<string>> Handle(ChangeUserPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.Id.ToString());
            if (user == null) return BadRequest<string>(_stringLocalizer[SharedResourcesKeys.NotFoundId]);
            var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
            if (!result.Succeeded) return BadRequest<string>(
             string.Join(" | ",
             result.Errors.Select(e => e.Description)));
            return Success("");
            #endregion
        }
    }
}
