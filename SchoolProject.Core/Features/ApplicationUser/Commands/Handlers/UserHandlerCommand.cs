using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using SchoolProject.Core.Bases;
using SchoolProject.Core.Features.ApplicationUser.Commands.Models;
using SchoolProject.Core.ShResources;
using SchoolProject.Data.Entities.Identity;

namespace SchoolProject.Core.Features.ApplicationUser.Commands.Handlers
{
    public class UserHandlerCommand : ResponseHandler,
                                                    IRequestHandler<AddUserCommand, Response<string>>
    {
        #region filde
        public readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        private readonly UserManager<User> _userManager;
        #endregion
        #region Constractor
        public UserHandlerCommand(IMapper mapper, IStringLocalizer<SharedResources> stringLocalizer, UserManager<User> userManager) : base(stringLocalizer)
        {
            _mapper = mapper;
            _stringLocalizer = stringLocalizer;
            _userManager = userManager;
        }
        #endregion
        #region Handelfunc
        public async Task<Response<string>> Handle(AddUserCommand request, CancellationToken cancellationToken)
        {
            //if Email is exist
            var email = await _userManager.FindByEmailAsync(request.Email);
            if (email != null) return BadRequest<string>(_stringLocalizer[SharedResourcesKeys.EmaliIsExist]);
            //if username is exist
            var username = await _userManager.FindByNameAsync(request.UserName);
            if (username != null) return BadRequest<string>(_stringLocalizer[SharedResourcesKeys.UserNameIsExist]);
            var mapper = _mapper.Map<User>(request);
            var createreult = await _userManager.CreateAsync(mapper, request.Password);
            if (createreult == null) return BadRequest<string>(createreult.Errors.FirstOrDefault().Description);
            return Created("");
        }
        #endregion
    }
}
