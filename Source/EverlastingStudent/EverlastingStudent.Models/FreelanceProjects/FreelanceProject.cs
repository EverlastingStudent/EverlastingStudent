namespace EverlastingStudent.Models.FreelanceProjects
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Newtonsoft.Json;

    // [Table("FreelanceProject")]
    public class FreelanceProject : BaseFreelanceProject
    {
        public FreelanceProject()
        {
        }

        [Key]
        public int FreelanceProjectId { get; set; }

        public int BaseFreelanceProjectId { get; set; }

        [ForeignKey("BaseFreelanceProjectId")]
        public virtual BaseFreelanceProject BaseFreelanceProject { get; set; }

        public object StudentId { get; set; }

        [JsonIgnore]
        public virtual Student Student { get; set; }

        public bool IsSolved { get; set; }

        public DateTime? StartDateTime { get; set; }

        public DateTime? LastWorkingDateTime { get; set; }

        public float ProgressInPercentage { get; set; }

        public float WorkPercentage { get; set; }
    }
}
