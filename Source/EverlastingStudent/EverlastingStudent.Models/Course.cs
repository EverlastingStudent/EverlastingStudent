namespace EverlastingStudent.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using EverlastingStudent.Common.Models;

    public class Course : DeletableEntity
    {
        private ICollection<StudentInCourses> studentInCourses;

        public Course()
        {
            this.studentInCourses = new HashSet<StudentInCourses>();
            this.Lectures = new HashSet<Lecture>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public int? NextCourseId { get; set; }

        public Course NextCourse { get; set; }

        public int? ExamId { get; set; }

        public virtual Exam Exam { get; set; }

        public virtual ICollection<StudentInCourses> StudentInCourses
        {
            get { return this.studentInCourses; }
            set { this.studentInCourses = value; }
        }

        public virtual ICollection<Lecture> Lectures { get; set; }
    }
}
