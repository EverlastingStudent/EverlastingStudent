namespace EverlastingStudent.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.Collections.Generic;

    using EverlastingStudent.Common.Models;

    public class Drink : DeletableEntity
    {
        private ICollection<Student> students;

        public Drink()
        {
            this.students = new HashSet<Student>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public int EnergyBonus { get; set; }

        public int MoneyCost { get; set; }

        public virtual ICollection<Student> Students
        {
            get { return this.students; }
            set { this.students = value; }
        }
    }
}
