using SchoolProject.Data.Commons;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolProject.Data.Entities
{
    public class Instructor : GeneralLocalizableEntity
    {
        public Instructor()
        {
            ListInstructors = new HashSet<Instructor>();
            InstructorSubjects = new HashSet<InstructorSubject>();
        }
        public int Id { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string Address { get; set; }
        public string? Position { get; set; }
        public int? SupervisorId { get; set; }
        public decimal Salary { get; set; }
        public int DId { get; set; }
        [ForeignKey(nameof(DId))]
        [InverseProperty(nameof(Department.Instructors))]
        public Department Department { get; set; }
        [InverseProperty(nameof(Department.InstructorMangers))]
        public Department DepartmentManager { get; set; }

        [ForeignKey(nameof(SupervisorId))]
        [InverseProperty("ListInstructors")]
        public Instructor Supervisor { get; set; }
        [InverseProperty("Supervisor")]
        public virtual ICollection<Instructor> ListInstructors { get; set; }

        [InverseProperty(nameof(InstructorSubject.Instructor))]
        public virtual ICollection<InstructorSubject> InstructorSubjects { get; set; }
    }
}
