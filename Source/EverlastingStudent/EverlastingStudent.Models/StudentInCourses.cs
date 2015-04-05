namespace EverlastingStudent.Models
{
    using System.ComponentModel.DataAnnotations;

    public class StudentInCourses
    {
        [Key]
        public int Id { get; set; }

        public int StudentId { get; set; }

        public virtual Student Student { get; set; }

        public int CourseId { get; set; }

        public Course Course { get; set; }

        public bool IsActive { get; set; }

        public bool IsPassed { get; set; }

        public bool IsPassedWithExcellence { get; set; }
    }
}
