namespace EverlastingStudent.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.Collections.Generic;

    using EverlastingStudent.Common.Models;

    public class HardwarePart : DeletableEntity
    {
        private ICollection<Student> students;

        public HardwarePart()
        {
            this.students = new HashSet<Student>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public int CoefficientEnergyBonus { get; set; }

        public int MoneyCost { get; set; }

        public virtual ICollection<Student> Students
        {
            get { return this.students; }
            set { this.students = value; }
        }
    }
}
