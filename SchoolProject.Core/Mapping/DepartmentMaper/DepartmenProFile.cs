using AutoMapper;

namespace SchoolProject.Core.Mapping.DepartmentMaper
{
    public partial class DepartmenProFile : Profile
    {
        public DepartmenProFile()
        {
            GetDepartmentByIdMapping();
        }
    }
}
