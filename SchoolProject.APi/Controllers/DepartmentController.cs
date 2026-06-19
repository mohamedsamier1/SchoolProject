using Microsoft.AspNetCore.Mvc;
using SchoolProject.APi.Base;
using SchoolProject.Core.Features.Department.Queries.Models;
using SchoolProject.Data.AppMetaData;

namespace SchoolProject.APi.Controllers
{

    [ApiController]
    public class DepartmentController : AppBaseController
    {
        [HttpGet(Router.DepartmentRouting.GetDepartmentById)]
        public async Task<IActionResult> GetDepartmentById([FromQuery] GetDepartmentByIdQuery query)
        {
            return NewResult(await _mediator.Send(query));
        }
    }
}
