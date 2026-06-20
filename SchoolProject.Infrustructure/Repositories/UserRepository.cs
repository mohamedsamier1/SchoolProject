using Microsoft.EntityFrameworkCore;
using SchoolProject.Data.Entities.Identity;
using SchoolProject.Infrustructure.Data;
using SchoolProject.Infrustructure.InfrustructureBases;
using SchoolProject.Infrustructure.IRepositories;

namespace SchoolProject.Infrustructure.Repositories
{
    public class UserRepository : GenericRepositoryAsync<User>, IUserRepository
    {
        #region Fields
        private readonly DbSet<User> _dbSubject;

        #endregion

        #region Constructors
        public UserRepository(ApplicationDbContext contxt) : base(contxt)
        {
            _dbSubject = contxt.Set<User>();
        }
        #endregion
        #region Handelfuntions

        #endregion
    }
}
