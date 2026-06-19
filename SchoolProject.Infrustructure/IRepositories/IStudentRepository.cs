using SchoolProject.Data.Entities;
using SchoolProject.Infrustructure.InfrustructureBases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProject.Infrustructure.IRepositories
{
    public interface IStudentRepository:IGenericRepositoryAsync<Student>
    {
        public Task<List<Student>> GeStudentsListAsunc();
    }
}
