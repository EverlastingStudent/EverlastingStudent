namespace EverlastingStudent.Models.FreelanceProjects
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using EverlastingStudent.Common.Contracts;

    [Table("BaseFreelanceProject")]
    public class BaseFreelanceProject : IDeletableEntity
    {
        public BaseFreelanceProject()
        {
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        public long RequireExperience { get; set; }

        public int EnergyCost { get; set; }

        public int MoneyGain { get; set; }

        public int ExperienceGain { get; set; }

        public int SolveDurabationInHours { get; set; }

        public bool IsActive { get; set; }

        public DateTime OpenForTakenDatetime { get; set; }

        public DateTime CloseForTakenDatetime { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}
