using AutoMapper;

namespace SchoolProject.Core.Mapping.StudentMaper
{
    public partial class StudentProfile : Profile
    {
        public StudentProfile()
        {
            GetStudnetListMapping();
            GetStudneByIdMapping();
            AddStudentCommandMapping();
            EditStudentCommandMapping();
        }
    }
}
