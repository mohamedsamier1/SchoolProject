using Microsoft.EntityFrameworkCore;
using SchoolProject.Data.Entities;
using SchoolProject.Infrustructure.Data;
using SchoolProject.Infrustructure.InfrustructureBases;
using SchoolProject.Infrustructure.IRepositories;

namespace SchoolProject.Infrustructure.Repositories
{
    public class DepartmentRepository : GenericRepositoryAsync<Department>, IDepartmentRepository
    {
        #region Fields
        private readonly DbSet<Department> _dbDepartment;

        #endregion

        #region Constructors
        public DepartmentRepository(ApplicationDbContext contxt) : base(contxt)
        {
            _dbDepartment = contxt.Set<Department>();
        }
        #endregion
        #region Handelfuntions

        #endregion
    }
}
