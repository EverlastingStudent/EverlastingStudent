namespace EverlastingStudent.Models.FreelanceProjects
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using EverlastingStudent.Models.Contracts;
    using EverlastingStudent.Models.FreelanceProjects;
    using Newtonsoft.Json;

    [Table("FreelanceProject")]
    public class FreelanceProject : BaseFreelanceProject, IDoable
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

        public virtual Student Student { get; set; }

        public bool IsSolved { get; set; }

        public DateTime? ActivatedDateTime { get; set; }

        public float ProgressInPercentage { get; set; }

        public static FreelanceProject CopyToFreelanceProject(BaseFreelanceProject source)
        {
            // Don't serialize a null object, simply return the default for that object
            if (ReferenceEquals(source, null))
            {
                return null;
            }

            return JsonConvert.DeserializeObject<FreelanceProject>(JsonConvert.SerializeObject(source));
        }

        public float Do()
        {
            if (!this.Student.FreelanceProjects.Contains(this))
            {
                throw new InvalidOperationException("You have to add project to wokr on it.");
            }

            if (this.IsDeleted)
            {
                throw new NullReferenceException("Can't work on deleted project.");
            }

            if (this.IsSolved)
            {
                throw new InvalidOperationException("Project is done.");
            }

            if (this.CloseForTakenDatetime <= DateTime.Now)
            {
                throw new InvalidOperationException("Project is no longer active.");
            }

            if (this.OpenForTakenDatetime > DateTime.Now)
            {
                throw new InvalidOperationException("Project is not active yet.");
            }

            if (this.Student.Experience < this.RequireExperience)
            {
                throw new InvalidOperationException("Not enough experience.");
            }

            if (this.Student.Energy < this.EnergyCost)
            {
                throw new InvalidOperationException("Not enough energy.");
            }


            // TODO: take user energy and gain poject progress
            return this.ProgressInPercentage;
        }
    }
}
