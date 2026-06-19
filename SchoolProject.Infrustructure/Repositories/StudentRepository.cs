using Microsoft.EntityFrameworkCore;
using SchoolProject.Data.Entities;
using SchoolProject.Infrustructure.Data;
using SchoolProject.Infrustructure.InfrustructureBases;
using SchoolProject.Infrustructure.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProject.Infrustructure.Repositories
{
    public class StudentRepository : GenericRepositoryAsync<Student>,IStudentRepository
    {
        #region Fields
        private readonly DbSet<Student>_dbStudent;

        #endregion

        #region Constructors
        public StudentRepository(ApplicationDbContext contxt): base(contxt) 
        {
            _dbStudent = contxt.Set<Student>();
        }
        #endregion


        #region Handles Functions
        public async Task<List<Student>> GeStudentsListAsunc()
        {
            return await _dbStudent.Include(n=>n.Department).ToListAsync();
        }
        #endregion

    }
}
