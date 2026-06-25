using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SchoolProject.Data.DTOS;
using SchoolProject.Data.Entities.Identity;
using SchoolProject.Data.Helpers;
using SchoolProject.Infrustructure.Data;
using SchoolProject.Service.Abstracts;
using System.Security.Claims;

namespace SchoolProject.Service.Implementations
{
    public class AuthorizationService : IAuthorizationService
    {
        #region Fields
        private readonly RoleManager<Role> _roleManager;
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _dbContext;
        #endregion
        #region Constructor
        public AuthorizationService(RoleManager<Role> roleManager, UserManager<User> userManager, ApplicationDbContext dbContext)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _dbContext = dbContext;
        }
        #endregion
        #region Handlefunc
        public async Task<string> AddRoleAsync(string roleName)
        {
            var identityRole = new Role();
            identityRole.Name = roleName;
            var result = await _roleManager.CreateAsync(identityRole);
            if (result.Succeeded)
            {
                return "Success";
            }
            return "Failed";
        }


        public async Task<string> EditRoleAsync(int id, string Name)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());
            if (role == null)
            {
                return "notFound";
            }
            role.Name = Name;
            var result = await _roleManager.UpdateAsync(role);
            if (result.Succeeded)
            {
                return "Success";

            }
            return string.Join(", ", result.Errors.Select(e => e.Description));

        }

        public async Task<bool> IsRoleExist(string roleName)
        {
            //var role = await _roleManager.FindByNameAsync(roleName);
            //if (role == null) return false;
            //return true;
            return await _roleManager.RoleExistsAsync(roleName);
        }

        public async Task<bool> IsRoleExistById(int roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId.ToString());
            if (role == null) return false;
            return true;
        }
        public async Task<string> DeletrRoleAsync(int Id)
        {
            var role = await _roleManager.FindByIdAsync(Id.ToString());
            if (role == null) return "NotFound";
            var users = await _userManager.GetUsersInRoleAsync(role.Name);
            if (users != null && users.Count() > 0) return "ThisRoleIsUsed";
            var result = await _roleManager.DeleteAsync(role);
            if (result.Succeeded) return "Success";
            else return string.Join(", ", result.Errors.Select(e => e.Description));
        }

        public async Task<List<Role>> GetRoleList()
        {
            return await _roleManager.Roles.ToListAsync();
        }

        public async Task<Role> GetRoleById(int id)
        {
            return await _roleManager.FindByIdAsync(id.ToString());
        }

        public async Task<ManageUserRolesDto> GetUserIdAndRole(User user)
        {
            var response = new ManageUserRolesDto();
            var roleList = new List<Roles>();

            var roles = await _roleManager.Roles.ToListAsync();
            response.UserId = user.Id;
            foreach (var role in roles)
            {
                var userrole = new Roles();
                userrole.Id = role.Id;
                userrole.Name = role.Name;
                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    userrole.HasRole = true;
                }
                else
                {
                    userrole.HasRole = false;

                }
                roleList.Add(userrole);
            }
            response.Roles = roleList;
            return response;
        }

        public async Task<string> UpdateUserRoles(UpdateUserRolesDto request)
        {
            var transact = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                var user = await _userManager.FindByIdAsync(request.UserId.ToString());
                if (user == null) return "UserIsNull";
                var userRoles = await _userManager.GetRolesAsync(user);
                var removeresult = await _userManager.RemoveFromRolesAsync(user, userRoles);
                if (!removeresult.Succeeded)
                {
                    return "FialedToRemoveOldRoles";
                }
                var selectedRoles = request.userRoles.Where(x => x.HasRole == true).Select(x => x.Name);
                var addroleresult = await _userManager.AddToRolesAsync(user, selectedRoles);
                if (!addroleresult.Succeeded) return "FialedToAddNewRoles";
                await transact.CommitAsync();
                return "Success";
            }
            catch (Exception ex)
            {
                await transact.RollbackAsync();
                return "FialedToUpdateUserRoles";
            }

        }
        public async Task<ManagUserClaimsDto> ManageUserClaimData(User user)
        {
            var response = new ManagUserClaimsDto();
            var usercliamsList = new List<UserClaims>();
            response.UsertId = user.Id;
            //Get User Claims
            //Check if claim exist for user then value = true

            var userclaims = await _userManager.GetClaimsAsync(user);
            foreach (var claim in ClaimStore.claims)
            {
                var usercliam = new UserClaims();
                usercliam.Type = claim.Type;
                if (userclaims.Any(x => x.Type == claim.Type))
                {
                    usercliam.Value = true;
                }
                else
                {
                    usercliam.Value = false;
                }
                usercliamsList.Add(usercliam);
            }
            //return result 
            response.userClaims = usercliamsList;
            return response;
        }

        public async Task<string> UpdateUserClaims(UpdateUserClaimsDto request)
        {
            var transact = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                var user = await _userManager.FindByIdAsync(request.UsertId.ToString());
                if (user == null) return "UserIsNull";
                var userclaims = await _userManager.GetClaimsAsync(user);
                var removeClaims = await _userManager.RemoveClaimsAsync(user, userclaims);
                if (!removeClaims.Succeeded)
                {
                    return "FialedToRemoveOldClaim";
                }
                var selectClaims = request.userClaims.Where(x => x.Value == true).Select(x => new Claim(x.Type, x.Value.ToString()));
                var addclaimsresult = await _userManager.AddClaimsAsync(user, selectClaims);
                if (!addclaimsresult.Succeeded) return "FialedToAddNewClaim";
                await transact.CommitAsync();
                return "Success";
            }
            catch (Exception ex)
            {
                await transact.RollbackAsync();
                return "FialedToUpdateUserClaim";
            }

        }
        #endregion

    }
}
