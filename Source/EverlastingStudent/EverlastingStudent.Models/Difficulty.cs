namespace EverlastingStudent.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using EverlastingStudent.Common;

    public class Difficulty
    {
        private ICollection<Homework> homeworks;
        private ICollection<FreelanceProject> freelanceProjects;


        public Difficulty()
        {
            this.homeworks = new HashSet<Homework>();
            this.freelanceProjects = new HashSet<FreelanceProject>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public int Value { get; set; }

        [Required]
        public TypeOfDifficulty type { get; set; }

        public virtual ICollection<Homework> Homeworks
        {
            get { return this.homeworks; }
            set { this.homeworks = value; }
        }

        public virtual ICollection<FreelanceProject> FreelanceProjects
        {
            get { return this.freelanceProjects; }
            set { this.freelanceProjects = value; }
        } 

        
    }
}
