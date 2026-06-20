using SchoolProject.Core.Features.ApplicationUser.Queries.Response;
using SchoolProject.Data.Entities.Identity;

namespace SchoolProject.Core.Mapping.ApplicationUserMapper
{
    public partial class ApplicationUserProfile
    {
        public void GetUserPaginationMapper()
        {
            CreateMap<User, GetUserListResponse>();

        }
    }
}
