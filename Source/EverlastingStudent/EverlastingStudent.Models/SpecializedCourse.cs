namespace EverlastingStudent.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class SpecializedCourse
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public float? InstantKnowledgeGain { get; set; }
                    
        public float? InstantExperienceGain { get; set; }
                    
        public float? CoefficientKnowledgeGain { get; set; }
                    
        public float? CoefficientExperienceGain { get; set; }
                    
        public float? CoefficientMoneyGain { get; set; }
                    
        public float? CoefficientEnergyLoss { get; set; }

        public bool IsActive { get; set; }

        public int MoneyCost { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }
}
