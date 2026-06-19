using Microsoft.EntityFrameworkCore;
using SchoolProject.Data.Entities;
using SchoolProject.Infrustructure.Data;
using SchoolProject.Infrustructure.InfrustructureBases;
using SchoolProject.Infrustructure.IRepositories;

namespace SchoolProject.Infrustructure.Repositories
{
    public class SubjectRepository : GenericRepositoryAsync<Subject>, ISubjectRepository
    {
        #region Fields
        private readonly DbSet<Subject> _dbSubject;

        #endregion

        #region Constructors
        public SubjectRepository(ApplicationDbContext contxt) : base(contxt)
        {
            _dbSubject = contxt.Set<Subject>();
        }
        #endregion
        #region Handelfuntions

        #endregion
    }
}
