namespace EverlastingStudent.Models
{
    public class StudentInCourses
    {
        public int StudentId { get; set; }

        public virtual Student Student { get; set; }

        public int CourseId { get; set; }

        public Course Course { get; set; }

        public bool IsActive { get; set; }

        public bool IsPassed { get; set; }

        public bool IsPassedWithExcellence { get; set; }
    }
}
