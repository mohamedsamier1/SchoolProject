using AutoMapper;

namespace SchoolProject.Core.Mapping.ApplicationUserMapper
{
    public partial class ApplicationUserProfile : Profile
    {
        public ApplicationUserProfile()
        {
            CreateUserMapping();
            GetUserPaginationMapper();
            GetUserByIdMapping();
            UpdateUserMapping();
        }
    }
}
