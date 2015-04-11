namespace EverlastingStudent.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using EverlastingStudent.Common.Models;

    public class StudentHomework
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string StudentId { get; set; }

        public virtual Student Student { get; set; }

        public int HomeworkId { get; set; }

        public virtual Homework Homework { get; set; }

        public int EnergyCost { get; set; }

        public double KnowledgeGain { get; set; }

        public double ExperienceGain { get; set; }

        public int SolveDurabationInMinutes { get; set; }

        public TypeOfDifficulty Type { get; set; }

        public bool IsSolved { get; set; }

        public DateTime CreatedOn { get; set; }

        public bool InProgress { get; set; }
    }
}
