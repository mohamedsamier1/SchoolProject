using Microsoft.AspNetCore.Mvc;
using SchoolProject.APi.Base;
using SchoolProject.Core.Features.Authentication.Commands.Models;
using SchoolProject.Core.Features.Authentication.Queries.Models;
using SchoolProject.Data.AppMetaData;

namespace SchoolProject.APi.Controllers
{

    [ApiController]
    public class AuthenticationController : AppBaseController
    {
        [HttpPost(Router.Authentication.SignIn)]
        public async Task<IActionResult> SignIn([FromBody] SignInCommand command)
        {
            var response = await _mediator.Send(command);
            return NewResult(response);
        }
        [HttpPost(Router.Authentication.RefreshToken)]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenCommand command)
        {
            var response = await _mediator.Send(command);
            return NewResult(response);
        }
        [HttpGet(Router.Authentication.ValidateToken)]
        public async Task<IActionResult> ValidateToken([FromQuery] AuthorizeUserQuery query)
        {
            var response = await _mediator.Send(query);
            return NewResult(response);
        }
    }
}
