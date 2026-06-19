using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolProject.Data.Entities
{
    public class DepartmentSubject
    {
        [Key]
        public int DeptId { get; set; }
        [Key]
        public int SubId { get; set; }
        [ForeignKey("DeptId")]
        [InverseProperty(nameof(Department.DepartmentSubjects))]
        public virtual Department Department { get; set; }
        [ForeignKey("SubId")]
        [InverseProperty(nameof(Subject.DepartmentSubjects))]
        public virtual Subject Subject { get; set; }
    }
}
