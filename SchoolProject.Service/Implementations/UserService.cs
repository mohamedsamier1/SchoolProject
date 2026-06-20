using SchoolProject.Infrustructure.IRepositories;
using SchoolProject.Service.Abstracts;

namespace SchoolProject.Service.Implementations
{
    public class UserService : IUserService
    {
        #region Fildes
        private readonly IUserRepository _userRepository;


        #endregion
        #region Constructor
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        #endregion
        #region Handel Func

        #endregion
    }
}
