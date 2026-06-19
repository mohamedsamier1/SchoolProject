using Microsoft.EntityFrameworkCore;
using SchoolProject.Data.Entities;
using SchoolProject.Infrustructure.IRepositories;
using SchoolProject.Service.Abstracts;

namespace SchoolProject.Service.Implementations
{
    public class DepartmentService : IDepartmentService
    {
        #region Filds
        private readonly IDepartmentRepository _departmentRepository;
        #endregion
        #region Conatructors
        public DepartmentService(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }
        #endregion
        #region Handel
        public async Task<Department> GetDepartmentById(int id)
        {
            var department = await _departmentRepository.GetTableNoTracking()
                                                 .Where(x => x.DId.Equals(id))
                                                 .Include(x => x.InstructorMangers)
                                                 .Include(x => x.Instructors)
                                                 .Include(x => x.DepartmentSubjects).ThenInclude(x => x.Subject)
                                                 .FirstOrDefaultAsync();
            return department;
        }

        public async Task<bool> IsDepartmentIdExist(int id)
        {
            return await _departmentRepository.GetTableNoTracking().AnyAsync(x => x.DId.Equals(id));
        }
        #endregion

    }
}
