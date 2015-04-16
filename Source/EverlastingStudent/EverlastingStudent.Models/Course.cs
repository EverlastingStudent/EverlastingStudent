namespace EverlastingStudent.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using EverlastingStudent.Common.Models;

    public class Course : DeletableEntity
    {
        private ICollection<StudentCourses> studentCourses;

        public Course()
        {
            this.studentCourses = new HashSet<StudentCourses>();
            this.Lectures = new HashSet<Lecture>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public virtual int? NextCourseId { get; set; }

        public virtual Course NextCourse { get; set; }

        public virtual Exam Exam { get; set; }

        public virtual ICollection<StudentCourses> StudentCourses
        {
            get { return this.studentCourses; }
            set { this.studentCourses = value; }
        }

        public virtual ICollection<Lecture> Lectures { get; set; }
    }
}
