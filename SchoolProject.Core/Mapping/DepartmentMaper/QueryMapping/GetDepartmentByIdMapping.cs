using SchoolProject.Core.Features.Department.Queries.Results;
using SchoolProject.Data.Entities;

namespace SchoolProject.Core.Mapping.DepartmentMaper
{
    public partial class DepartmenProFile
    {
        public void GetDepartmentByIdMapping()
        {
            CreateMap<Department, GetDepartmentByIdResponse>()
                .ForMember(des => des.Name, opt => opt.MapFrom(src => src.GenerallLocalizedName(src.DNameAr, src.DNameEn)))
                .ForMember(des => des.DId, opt => opt.MapFrom(src => src.DId))
                .ForMember(des => des.ManagerName, opt => opt.MapFrom(src => src.InstructorMangers.GenerallLocalizedName(src.DNameAr, src.DNameEn)))
                .ForMember(des => des.SubjectList, opt => opt.MapFrom(src => src.DepartmentSubjects))
                // .ForMember(des => des.StudentList, opt => opt.MapFrom(src => src.Students))
                .ForMember(des => des.InstructortList, opt => opt.MapFrom(src => src.Instructors));
            CreateMap<DepartmentSubject, SubjectResponse>()
                 .ForMember(des => des.Id, opt => opt.MapFrom(src => src.SubId))
                 .ForMember(des => des.Name, opt => opt.MapFrom(src => src.Subject.GenerallLocalizedName(src.Subject.SubjectNameAr, src.Subject.SubjectNameEn)));
            //CreateMap<Student, StudentResponse>()
            //    .ForMember(des => des.Id, opt => opt.MapFrom(src => src.Id))
            //    .ForMember(des => des.Name, opt => opt.MapFrom(src => src.GenerallLocalizedName(src.NameAr, src.NameEn)));
            CreateMap<Instructor, InstructorResponse>()
                .ForMember(des => des.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(des => des.Name, opt => opt.MapFrom(src => src.GenerallLocalizedName(src.NameAr, src.NameEn)));


        }
    }
}
