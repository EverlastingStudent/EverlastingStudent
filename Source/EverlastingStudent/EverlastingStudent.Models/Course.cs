namespace EverlastingStudent.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Course
    {
        private ICollection<Student> students;

        public Course()
        {
            this.students = new HashSet<Student>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public bool IsActive { get; set; }

        public int ExamId { get; set; }

        public virtual Exam Exam { get; set; }

        public virtual ICollection<Student> Students
        {
            get { return this.students; }
            set { this.students = value; }
        }
    }
}
