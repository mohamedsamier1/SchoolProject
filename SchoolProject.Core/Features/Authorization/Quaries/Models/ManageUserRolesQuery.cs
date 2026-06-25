using MediatR;
using SchoolProject.Core.Bases;
using SchoolProject.Data.DTOS;

namespace SchoolProject.Core.Features.Authorization.Quaries.Models
{
    public class ManageUserRolesQuery : IRequest<Response<ManageUserRolesDto>>
    {
        public int UserId { get; set; }

        public ManageUserRolesQuery(int userId)
        {
            UserId = userId;
        }
    }
}
