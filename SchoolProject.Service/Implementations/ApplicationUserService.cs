using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SchoolProject.Data.Entities.Identity;
using SchoolProject.Infrustructure.Data;
using SchoolProject.Service.Abstracts;

namespace SchoolProject.Service.Implementations
{
    public class ApplicationUserService : IApplicationUserService
    {
        #region field
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEmailService _emailService;
        private readonly ApplicationDbContext _applicationDbContext;
        public readonly IUrlHelper _urlHelper;

        #endregion
        #region Constructor
        public ApplicationUserService(UserManager<User> userManager, IEmailService emailService, IHttpContextAccessor httpContextAccessor, ApplicationDbContext applicationDbContext, IUrlHelper urlHelper)
        {
            _userManager = userManager;
            _emailService = emailService;
            _httpContextAccessor = httpContextAccessor;
            _applicationDbContext = applicationDbContext;
            _urlHelper = urlHelper;
        }
        #endregion
        #region Handel Functions
        public async Task<string> AddUserAsync(User user, string Password)
        {
            var trans = await _applicationDbContext.Database.BeginTransactionAsync();
            try
            {
                //if Email is exist
                var email = await _userManager.FindByEmailAsync(user.Email);
                if (email != null) return "EmaliIsExist";
                //if username is exist
                var username = await _userManager.FindByNameAsync(user.UserName);
                if (username != null) return "UserNameIsExist";
                var createreult = await _userManager.CreateAsync(user, Password);
                if (!createreult.Succeeded) return string.Join(",", createreult.Errors.Select(x => x.Description).ToList());
                await _userManager.AddToRoleAsync(user, "User");
                //send confirm email
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var requestAccessor = _httpContextAccessor.HttpContext.Request;
                var returnUrl = requestAccessor.Scheme + "://" + requestAccessor.Host + _urlHelper.Action("ConfirmEmail", "Authentication", new { userId = user.Id, code = code });
                var message = $"To Confirm Email Click Link <a href='{returnUrl}'></a>";
                //  $"/Api/v1/Authentication/ConfirmEmail?userId={user.Id}&code={code}";
                var sendEmailResult = await _emailService.SendEmail(user.Email, message, "Confirm Email");
                await trans.CommitAsync();
                return "Success";
            }
            catch (Exception ex)
            {
                await trans.RollbackAsync();
                return "Failed";
            }

        }
        #endregion

    }
}
