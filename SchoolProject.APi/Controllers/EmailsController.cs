using Microsoft.AspNetCore.Mvc;
using SchoolProject.APi.Base;
using SchoolProject.Core.Features.Emails.Commands.Models;
using SchoolProject.Data.AppMetaData;

namespace SchoolProject.APi.Controllers
{

    [ApiController]
    public class EmailsController : AppBaseController
    {
        [HttpPost(Router.Emails.SendEmail)]
        public async Task<IActionResult> SendEmail([FromQuery] SendEmailCommand command)
        {
            var resutl = await _mediator.Send(command);
            return NewResult(resutl);
        }
    }
}
