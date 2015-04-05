namespace EverlastingStudent.Models
{
    public class StudentHomework
    {
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
