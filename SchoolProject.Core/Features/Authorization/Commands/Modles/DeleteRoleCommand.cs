using MediatR;
using SchoolProject.Core.Bases;

namespace SchoolProject.Core.Features.Authorization.Commands.Modles
{
    public class DeleteRoleCommand : IRequest<Response<string>>
    {
        public int Id { get; set; }

        public DeleteRoleCommand(int id)
        {
            Id = id;
        }


    }
}
