using MediatR;
using SchoolProject.Core.Bases;
using SchoolProject.Data.DTOS;

namespace SchoolProject.Core.Features.Authorization.Quaries.Models
{
    public class ManageUserClaimsQuery : IRequest<Response<ManagUserClaimsDto>>
    {
        public int UserId { get; set; }
    }
}
