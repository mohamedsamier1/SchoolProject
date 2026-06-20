using Microsoft.AspNetCore.Mvc;
using SchoolProject.APi.Base;
using SchoolProject.Core.Features.ApplicationUser.Commands.Models;
using SchoolProject.Core.Features.ApplicationUser.Queries.Models;
using SchoolProject.Data.AppMetaData;

namespace SchoolProject.APi.Controllers
{

    [ApiController]
    public class ApplicationUserController : AppBaseController
    {
        [HttpPost(Router.UserRouting.CreateUser)]
        public async Task<IActionResult> CreateUser([FromBody] AddUserCommand command)
        {
            var result = await _mediator.Send(command);
            return NewResult(result);
        }
        [HttpGet(Router.UserRouting.PaginatedUser)]
        public async Task<IActionResult> GetPaginatedUser([FromQuery] GetUserPaginationQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        [HttpGet(Router.UserRouting.GetUserById)]
        public async Task<IActionResult> GetUserById([FromRoute] int id)
        {
            var resutl = await _mediator.Send(new GetUserByIdQuery(id));
            return NewResult(resutl);
        }
        [HttpPut(Router.UserRouting.EditUser)]
        public async Task<IActionResult> UpdatedUser([FromBody] UpdateUserCommand command)
        {
            var response = await _mediator.Send(command);
            return NewResult(response);
        }
    }
}
