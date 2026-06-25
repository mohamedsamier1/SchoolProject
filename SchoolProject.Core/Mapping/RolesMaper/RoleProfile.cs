using AutoMapper;

namespace SchoolProject.Core.Mapping.RolesMaper
{
    public partial class RoleProfile : Profile
    {
        public RoleProfile()
        {
            GetRoleListMapping();
            GetRoleByIdMapping();
        }
    }
}
