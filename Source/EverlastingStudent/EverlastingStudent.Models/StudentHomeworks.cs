﻿namespace EverlastingStudent.Models
{
    using System.ComponentModel.DataAnnotations;

    public class StudentHomework
    {
        [Key]
        public int Id { get; set; }

        public string StudentId { get; set; }

        public virtual Student Student { get; set; }

        public int HomeworkId { get; set; }

        public virtual Homework Homework { get; set; }

        public int EnergyCost { get; set; }

        public int KnowledgeGain { get; set; }

        public int ExperienceGain { get; set; }

        public int SolveDurabationInMinutes { get; set; }

        public bool IsSolved { get; set; }
    }
}
