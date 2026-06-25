using MediatR;
using SchoolProject.Core.Bases;
using SchoolProject.Core.Features.Authorization.Quaries.Result;

namespace SchoolProject.Core.Features.Authorization.Quaries.Models
{
    public class GetRoleListQuery : IRequest<Response<List<GetRoleResponse>>>
    {
    }
}
