namespace EverlastingStudent.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Drink
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int Name { get; set; }

        public int EnergyBonus { get; set; }

        public int MoneyCost { get; set; }
    }
}
