using MediatR;
using SchoolProject.Core.Bases;
using SchoolProject.Data.DTOS;

namespace SchoolProject.Core.Features.Authorization.Commands.Modles
{
    public class UpdateUserRolesCommand : UpdateUserRolesDto, IRequest<Response<string>>
    {

    }

}
