using MediatR;
using SchoolProject.Core.Bases;

namespace SchoolProject.Core.Features.Authorization.Commands.Modles
{
    public class EditRoleCommand : IRequest<Response<string>>
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
