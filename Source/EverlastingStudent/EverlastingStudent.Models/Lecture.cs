namespace EverlastingStudent.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using EverlastingStudent.Common.Models;

    public class Lecture : DeletableEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public int CourseId { get; set; }

        public virtual Course Course { get; set; }

        public int DurationInMinutes { get; set; }

        public double CoefficientKnowledgeGain { get; set; }

        public virtual ICollection<StudentLectures> StudentLectures { get; set; }
    }
}
