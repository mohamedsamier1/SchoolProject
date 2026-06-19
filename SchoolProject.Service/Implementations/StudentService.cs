using Microsoft.EntityFrameworkCore;
using SchoolProject.Data.Entities;
using SchoolProject.Data.Helpers;
using SchoolProject.Infrustructure.IRepositories;
using SchoolProject.Service.Abstracts;
using System.Globalization;

namespace SchoolProject.Service.Implementations
{
    public class StudentService : IStudentService
    {
        #region Filds
        private readonly IStudentRepository _studentRepository;
        #endregion
        #region Conatructors
        public StudentService(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }
        #endregion
        #region Handl Functions
        public async Task<List<Student>> GetStudentListAsync()
        {
            return await _studentRepository.GeStudentsListAsunc();
        }
        public async Task<Student> GetStudentByIdAsync(int id)
        {
            //var stu =await _studentRepository.GetByIdAsync(id);   
            var student = _studentRepository.GetTableNoTracking()
                                           .Include(d => d.Department)
                                           .Where(d => d.Id == id)
                                           .FirstOrDefault();
            return student;
        }
        public async Task<string> CreatNewStudentAsync(Student student)
        {
            // chek if the name is exist or not
            bool isArabic =
                CultureInfo.CurrentCulture.TwoLetterISOLanguageName == "ar";

            var studentName = isArabic
                ? student.NameAr
                : student.NameEn;

            var studentname = await _studentRepository
                .GetTableNoTracking()
                .FirstOrDefaultAsync(x =>
                    (isArabic ? x.NameAr : x.NameEn) == studentName); if (studentname != null)
                return "Exist";
            var isvalidId = await _studentRepository.GetTableNoTracking().AnyAsync(s => s.DID == student.DID);
            if (!isvalidId)
                return "Invalid Department Id";
            await _studentRepository.AddAsync(student);
            return "Success";
        }
        public async Task<string> EditStudentAsync(Student student)
        {
            var isvalidId = await _studentRepository.GetTableNoTracking().AnyAsync(s => s.DID == student.DID);
            if (!isvalidId)
                return "Invalid Department Id";
            await _studentRepository.UpdateAsync(student);
            return "Success";
        }
        public async Task<string> DeletStudentAsyAsync(Student student)
        {
            var trans = _studentRepository.BeginTransaction();
            try
            {
                await _studentRepository.DeleteAsync(student);
                await trans.CommitAsync();
                return "Deleted Success";
            }
            catch (Exception)
            {
                await trans.RollbackAsync();
                return "Falied";
            }

        }
        public IQueryable<Student> GetStudentQuerable()
        {
            return _studentRepository.GetTableNoTracking()
                                          .Include(d => d.Department)
                                          .AsQueryable();
        }
        public IQueryable<Student> FilterStuedntPaginatedQuerable(StudentOrderingEnum orderingEnum, string search)
        {
            bool isArabic =
                CultureInfo.CurrentCulture.TwoLetterISOLanguageName == "ar";

            var querable = _studentRepository.GetTableNoTracking()
                                            .Include(d => d.Department)
                                            .AsQueryable();
            if (search != null)
            {
                querable = querable.Where(x =>
                    ((isArabic ? x.NameAr : x.NameEn) ?? "")
                        .Contains(search)
                    ||
                    ((x.Address ?? ""))
                        .Contains(search));
            }
            switch (orderingEnum)
            {
                case StudentOrderingEnum.Id:
                    querable = querable.OrderBy(x => x.Id);
                    break;
                case StudentOrderingEnum.Name:
                    querable = isArabic
                                 ? querable.OrderBy(x => x.NameAr)
                                 : querable.OrderBy(x => x.NameEn);
                    break;
                case StudentOrderingEnum.Address:
                    querable = querable.OrderBy(x => x.Address);
                    break;
                case StudentOrderingEnum.DepatmentName:
                    querable = isArabic
                                  ? querable.OrderBy(x => x.Department!.DNameAr)
                                  : querable.OrderBy(x => x.Department!.DNameEn);
                    break;
            }

            return querable;
        }

        public IQueryable<Student> GetStudentByDepartmentIdQuerable(int Did)
        {
            return _studentRepository.GetTableNoTracking()
                                       .Where(e => e.DID.Equals(Did))
                                         .AsQueryable();
        }




        #endregion

    }
}
