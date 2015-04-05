namespace EverlastingStudent.Models
{
    using System.ComponentModel.DataAnnotations;

    public class StudentFreelanceProject
    {
        [Key]
        public int Id { get; set; }

        public string StudentId { get; set; }

        public virtual Student Student { get; set; }

        public int FreelanceProjectId { get; set; }

        public virtual FreelanceProject FreelanceProject { get; set; }

        public int EnergyCost { get; set; }

        public int MoneyGain { get; set; }

        public int ExperienceGain { get; set; }

        public int SolveDurabationInMinutes { get; set; }

        public bool IsSolved { get; set; }
    }
}
