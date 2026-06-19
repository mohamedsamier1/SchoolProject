using Microsoft.EntityFrameworkCore;
using SchoolProject.Data.Entities;
using SchoolProject.Infrustructure.Data;
using SchoolProject.Infrustructure.InfrustructureBases;
using SchoolProject.Infrustructure.IRepositories;

namespace SchoolProject.Infrustructure.Repositories
{
    public class InstructorsRepository : GenericRepositoryAsync<Instructor>, IInstructorsRepository
    {
        #region Fields
        private readonly DbSet<Instructor> _dbInstructor;

        #endregion

        #region Constructors
        public InstructorsRepository(ApplicationDbContext contxt) : base(contxt)
        {
            _dbInstructor = contxt.Set<Instructor>();
        }
        #endregion
        #region Handelfuntions

        #endregion
    }
}
