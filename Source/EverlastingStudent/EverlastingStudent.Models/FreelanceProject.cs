namespace EverlastingStudent.Models
{
    using System.ComponentModel.DataAnnotations;

    public class FreelanceProject
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }
    }
}
