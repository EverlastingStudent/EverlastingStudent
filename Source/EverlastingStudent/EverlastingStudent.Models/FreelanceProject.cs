namespace EverlastingStudent.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class FreelanceProject
    {
        private ICollection<Difficulty> difficulties;

        public FreelanceProject()
        {
            this.difficulties = new HashSet<Difficulty>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        public virtual ICollection<Difficulty> Difficulties
        {
            get { return this.difficulties; }
            set { this.difficulties = value; }
        }

    }
}
