using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using SchoolProject.Core.Bases;
using SchoolProject.Core.Features.Emails.Commands.Models;
using SchoolProject.Core.ShResources;
using SchoolProject.Service.Abstracts;

namespace SchoolProject.Core.Features.Emails.Commands.Handlers
{
    public class EmailsCommandHandler : ResponseHandler,
                                      IRequestHandler<SendEmailCommand, Response<string>>
    {
        #region Fildes
        private readonly IEmailService _emailService;
        public readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        #endregion
        #region Constructor
        public EmailsCommandHandler(IMapper mapper, IStringLocalizer<SharedResources> stringLocalizer, IEmailService emailService) : base(stringLocalizer)
        {

            _mapper = mapper;
            _stringLocalizer = stringLocalizer;
            _emailService = emailService;
        }


        #endregion
        #region HandelFunction
        public async Task<Response<string>> Handle(SendEmailCommand request, CancellationToken cancellationToken)
        {
            var response = await _emailService.SendEmail(request.Email, request.Message, null);

            if (response == "Success") return Success<string>("");
            return BadRequest<string>(_stringLocalizer[SharedResourcesKeys.sendEmailFailed]);
        }

        #endregion
    }
}