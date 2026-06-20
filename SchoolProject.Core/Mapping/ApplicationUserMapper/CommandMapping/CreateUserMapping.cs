using SchoolProject.Core.Features.ApplicationUser.Commands.Models;
using SchoolProject.Data.Entities.Identity;

namespace SchoolProject.Core.Mapping.ApplicationUserMapper
{
    public partial class ApplicationUserProfile
    {
        public void CreateUserMapping()
        {
            CreateMap<AddUserCommand, User>()
                .ForMember(des => des.PhoneNumber, opt => opt.MapFrom(src => src.Phone));
        }
    }
}
