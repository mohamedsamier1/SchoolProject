using Microsoft.EntityFrameworkCore;
using SchoolProject.Data.Entities.Identity;
using SchoolProject.Infrustructure.Data;
using SchoolProject.Infrustructure.InfrustructureBases;
using SchoolProject.Infrustructure.IRepositories;

namespace SchoolProject.Infrustructure.Repositories
{
    public class RefreshTokenRepository : GenericRepositoryAsync<UserRefreshToken>, IRefreshTokenRepository
    {
        #region Filde
        private readonly DbSet<UserRefreshToken> _userRefreshToken;

        #endregion
        #region Constructor
        public RefreshTokenRepository(ApplicationDbContext contxt) : base(contxt)
        {
            _userRefreshToken = contxt.Set<UserRefreshToken>();
        }

        #endregion
        #region Handel func

        #endregion

    }
}
