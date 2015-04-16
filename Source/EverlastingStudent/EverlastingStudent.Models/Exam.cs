namespace EverlastingStudent.Models
{
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Exam
    {
        [ForeignKey("Course")]
        public int Id { get; set; }

        [Required]
        public long RequireExpForExam { get; set; }

        public virtual Course Course { get; set; }

        public int ExamDurationInMinutes { get; set; }
    }
}
