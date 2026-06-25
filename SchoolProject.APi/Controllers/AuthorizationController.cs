using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolProject.APi.Base;
using SchoolProject.Core.Features.Authorization.Commands.Modles;
using SchoolProject.Core.Features.Authorization.Quaries.Models;
using SchoolProject.Data.AppMetaData;
using Swashbuckle.AspNetCore.Annotations;

namespace SchoolProject.APi.Controllers
{

    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AuthorizationController : AppBaseController
    {
        [HttpPost(Router.Authorization.Create)]
        public async Task<IActionResult> CreateRole([FromForm] AddRoleCommand command)
        {
            var result = await _mediator.Send(command);
            return NewResult(result);
        }
        [HttpPost(Router.Authorization.Edit)]
        public async Task<IActionResult> EditRole([FromForm] EditRoleCommand command)
        {
            var result = await _mediator.Send(command);
            return NewResult(result);
        }
        [HttpDelete(Router.Authorization.Delete)]
        public async Task<IActionResult> DeleteRole([FromRoute] int id)
        {
            var result = await _mediator.Send(new DeleteRoleCommand(id));
            return NewResult(result);
        }
        [HttpGet(Router.Authorization.GetRoleList)]
        public async Task<IActionResult> GetRoleList()
        {
            var result = await _mediator.Send(new GetRoleListQuery());
            return Ok(result);
        }
        [HttpGet(Router.Authorization.GetRoleById)]
        public async Task<IActionResult> GetRoleById([FromRoute] int id)
        {
            var result = await _mediator.Send(new GetRoleByIdQuery(id));
            return Ok(result);
        }
        [HttpGet(Router.Authorization.ManageUserRoles)]
        public async Task<IActionResult> ManageUserRoles([FromRoute] int id)
        {
            var result = await _mediator.Send(new ManageUserRolesQuery(id));
            return Ok(result);
        }
        [SwaggerOperation(Summary = "تعديل صلاحيات المستخدمين", OperationId = "UpdateUserRoles")]
        [HttpPut(Router.Authorization.UpdateUserRoles)]
        public async Task<IActionResult> UpdateUserRoles([FromBody] UpdateUserRolesCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        [SwaggerOperation(Summary = "اداره صلاحيات استخدام المستخدمين", OperationId = "ManageUserClaim")]
        [HttpGet(Router.Authorization.ManageUserClaim)]
        public async Task<IActionResult> ManageUserClaim([FromRoute] int id)
        {
            var result = await _mediator.Send(new ManageUserClaimsQuery() { UserId = id });
            return Ok(result);
        }

        [SwaggerOperation(Summary = "تعديل صلاحيات استخدام المستخدمين", OperationId = "UpdateUserClaim")]
        [HttpPut(Router.Authorization.UpdateUserClaim)]
        public async Task<IActionResult> UpdateUserClaim([FromBody] UpdateUserClaimsCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
