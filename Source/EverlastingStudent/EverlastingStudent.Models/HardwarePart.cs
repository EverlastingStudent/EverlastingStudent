namespace EverlastingStudent.Models
{
    using System.ComponentModel.DataAnnotations;

    using EverlastingStudent.Common.Models;

    public class HardwarePart : DeletableEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int Name { get; set; }

        public int CoefficientEnergyBonus { get; set; }

        public int MoneyCost { get; set; }
    }
}
