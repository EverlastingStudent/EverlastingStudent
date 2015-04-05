namespace EverlastingStudent.Models
{
    public class StudentFreelanceProject
    {

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
