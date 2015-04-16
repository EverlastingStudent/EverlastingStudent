namespace EverlastingStudent.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class StudentLectures
    {
        [Key]
        public int Id { get; set; }

        public string StudentId { get; set; }

        public virtual Student Student { get; set; }

        public int LectureId { get; set; }

        public virtual Lecture Lecture { get; set; }

        public bool IsActive { get; set; }

        public bool IsPassed { get; set; }

        public DateTime? PassedOn { get; set; }
    }
}
