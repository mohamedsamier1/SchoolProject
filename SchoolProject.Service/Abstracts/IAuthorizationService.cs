using SchoolProject.Data.DTOS;
using SchoolProject.Data.Entities.Identity;

namespace SchoolProject.Service.Abstracts
{
    public interface IAuthorizationService
    {
        public Task<string> AddRoleAsync(string roleName);
        public Task<string> EditRoleAsync(int id, string Name);
        public Task<bool> IsRoleExist(string roleName);
        public Task<bool> IsRoleExistById(int roleId);
        public Task<string> DeletrRoleAsync(int Id);
        public Task<List<Role>> GetRoleList();
        public Task<Role> GetRoleById(int id);
        public Task<ManageUserRolesDto> GetUserIdAndRole(User user);
        public Task<string> UpdateUserRoles(UpdateUserRolesDto request);
        public Task<string> UpdateUserClaims(UpdateUserClaimsDto request);
        public Task<ManagUserClaimsDto> ManageUserClaimData(User user);

    }
}
