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
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public int NextCourceId { get; set; }

        public Course NextCource { get; set; }

        public int ExamId { get; set; }

        public virtual Exam Exam { get; set; }

        public virtual ICollection<StudentInCourses> StudentInCourses
        {
            get { return this.studentInCourses; }
            set { this.studentInCourses = value; }
        }
    }
}
