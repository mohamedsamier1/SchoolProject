using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using SchoolProject.Core.Bases;
using SchoolProject.Core.Features.Authentication.Commands.Models;
using SchoolProject.Core.ShResources;
using SchoolProject.Data.Entities.Identity;
using SchoolProject.Data.Helpers;
using SchoolProject.Service.Abstracts;

namespace SchoolProject.Core.Features.Authentication.Commands.Handlers
{
    public class AuthenticationCommandHandler : ResponseHandler,
                                                IRequestHandler<SignInCommand, Response<JwtAuthResult>>,
                                                IRequestHandler<RefreshTokenCommand, Response<JwtAuthResult>>
    {
        #region filde
        public readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IAuthenticationService _authenticationService;
        #endregion
        #region Constractor
        public AuthenticationCommandHandler(IMapper mapper, IStringLocalizer<SharedResources> stringLocalizer, UserManager<User> userManager, SignInManager<User> signInManager, IAuthenticationService authenticationService) : base(stringLocalizer)
        {
            _mapper = mapper;
            _stringLocalizer = stringLocalizer;
            _userManager = userManager;
            _signInManager = signInManager;
            _authenticationService = authenticationService;
        }
        #endregion
        #region Handle fuc
        public async Task<Response<JwtAuthResult>> Handle(SignInCommand request, CancellationToken cancellationToken)
        {
            //check if user is exist or  not 
            var user = await _userManager.FindByNameAsync(request.UserName);
            //return the user name not found 
            if (user == null) return BadRequest<JwtAuthResult>(_stringLocalizer[SharedResourcesKeys.UserNameIsNotExist]);

            //try to sign in 
            var signinresult = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
            //if failed return passord is wrong
            if (!signinresult.Succeeded) return BadRequest<JwtAuthResult>(_stringLocalizer[SharedResourcesKeys.UserNameOrPasswordNotCorrect]);
            //generate token 
            var result = await _authenticationService.GetJwTToken(user);
            return Success(result);
            //return token
        }

        public async Task<Response<JwtAuthResult>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var jwtToken = _authenticationService.ReadJwtToken(request.AccessToken);
            var userIdAndExpirydate = await _authenticationService.ValidateDetails(jwtToken, request.AccessToken, request.RefreshToken);
            switch (userIdAndExpirydate)
            {
                case ("AlgorithmsIsWrong", null): return Unauthorized<JwtAuthResult>(_stringLocalizer[SharedResourcesKeys.AlgorithmsIsWrong]);
                case ("Tokenisnotexpired", null):
                    return Unauthorized<JwtAuthResult>(_stringLocalizer[SharedResourcesKeys.Tokenisnotexpired]);
                case ("refreshTokenisnotFound", null):
                    return Unauthorized<JwtAuthResult>(_stringLocalizer[SharedResourcesKeys.refreshTokenisnotFound]);
                case ("refreshTokenisexpired", null):
                    return Unauthorized<JwtAuthResult>(_stringLocalizer[SharedResourcesKeys.refreshTokenisexpired]);
                default:
                    break;
            }
            var (userId, expiryDate) = userIdAndExpirydate;
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound<JwtAuthResult>(_stringLocalizer[SharedResourcesKeys.NotFoundId]);
            var result = await _authenticationService.GetRefreshToken(user, jwtToken, expiryDate, request.RefreshToken);
            return Success(result);
        }
        #endregion
    }
}
