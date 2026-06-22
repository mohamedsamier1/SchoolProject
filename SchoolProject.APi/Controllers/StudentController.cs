using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolProject.APi.Base;
using SchoolProject.Core.Features.Students.Commands.Models;
using SchoolProject.Core.Features.Students.Queries.Models;
using SchoolProject.Data.AppMetaData;

namespace SchoolProject.APi.Controllers
{

    [ApiController]
    [Authorize]
    public class StudentController : AppBaseController
    {
        #region End Point
        [HttpGet(Router.StudentRouting.GetStudentlist)]
        public async Task<IActionResult> GetStuedntList()
        {
            var response = await _mediator.Send(new GetStudentListQuery());
            return Ok(response);
        }
        [AllowAnonymous]
        [HttpGet(Router.StudentRouting.PaginatedStudent)]
        public async Task<IActionResult> Paginated([FromQuery] GetStudentPaginatedListQuery query)
        {
            var response = await _mediator.Send(query);
            return Ok(response);
        }

        [HttpGet(Router.StudentRouting.GetStudentbyid)]
        public async Task<IActionResult> GetStudentByid([FromRoute] int id)
        {
            var response = await _mediator.Send(new GetStudentByIdQuery(id));
            return NewResult(response);
        }
        [HttpPost(Router.StudentRouting.CreateStudent)]
        public async Task<IActionResult> CreateStudent([FromBody] AddStudentCommand command)
        {
            var response = await _mediator.Send(command);
            return NewResult(response);
        }
        [HttpPut(Router.StudentRouting.EditStudent)]
        public async Task<IActionResult> EditStudent([FromBody] EditStudentCommand command)
        {
            var response = await _mediator.Send(command);
            return NewResult(response);
        }
        [HttpDelete(Router.StudentRouting.DeleteStudent)]
        public async Task<IActionResult> DeleteStudent([FromRoute] int id)
        {
            var response = await _mediator.Send(new DeleteStudentCommand(id));
            return NewResult(response);
        }
        #endregion

    }
}
