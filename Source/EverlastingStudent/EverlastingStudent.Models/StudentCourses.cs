namespace EverlastingStudent.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class StudentCourses
    {
        [Key]
        public int Id { get; set; }

        public string StudentId { get; set; }

        public virtual Student Student { get; set; }

        public int CourseId { get; set; }

        public virtual Course Course { get; set; }

        public bool IsActive { get; set; }

        public bool ExamInProgress { get; set; }

        public bool IsPassed { get; set; }

        public bool IsPassedWithExcellence { get; set; }

        public DateTime? PassedOn { get; set; }
    }
}
