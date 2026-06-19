using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolProject.Data.Entities
{
    public class StudentSubject
    {
        [Key]
        public int StudId { get; set; }
        [Key]
        public int SubId { get; set; }
        public decimal? grade { get; set; }
        [ForeignKey("StudId")]
        [InverseProperty(nameof(Student.StudentSubjects))]
        public virtual Student? Student { get; set; }
        [ForeignKey("SubId")]
        [InverseProperty(nameof(Subject.StudentSubjects))]
        public virtual Subject? Subject { get; set; }
    }
}
