using SchoolProject.Core.Features.Authorization.Quaries.Result;
using SchoolProject.Data.Entities.Identity;

namespace SchoolProject.Core.Mapping.RolesMaper
{
    public partial class RoleProfile
    {
        public void GetRoleByIdMapping()
        {
            CreateMap<Role, GetRoleByIdResponse>()
               .ForMember(des => des.Id, opt => opt.MapFrom(src => src.Id))
               .ForMember(des => des.RoleName, opt => opt.MapFrom(src => src.Name));
        }
    }
}
