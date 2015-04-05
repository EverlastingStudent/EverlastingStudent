namespace EverlastingStudent.Models
{
    using System.ComponentModel.DataAnnotations;

    public class HardwarePart
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int Name { get; set; }

        public int CoefficientEnergyBonus { get; set; }

        public int MoneyCost { get; set; }
    }
}
