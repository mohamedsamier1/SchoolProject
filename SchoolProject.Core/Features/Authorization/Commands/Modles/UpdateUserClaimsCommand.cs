using MediatR;
using SchoolProject.Core.Bases;
using SchoolProject.Data.DTOS;

namespace SchoolProject.Core.Features.Authorization.Commands.Modles
{
    public class UpdateUserClaimsCommand : UpdateUserClaimsDto, IRequest<Response<string>>
    {
    }
}
