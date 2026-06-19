using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolProject.Data.Entities
{
    public class InstructorSubject
    {
        [Key]
        public int IntructorId { get; set; }
        [Key]
        public int SubjectId { get; set; }
        [InverseProperty(nameof(Instructor.InstructorSubjects))]
        public Instructor Instructor { get; set; }
        [InverseProperty(nameof(Subject.InstructorSubjects))]
        public Subject Subject { get; set; }
    }
}
