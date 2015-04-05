﻿namespace EverlastingStudent.Models
{
    using System.ComponentModel.DataAnnotations;

    using EverlastingStudent.Common.Models;

    public class FreelanceProject : DeletableEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }
    }
}
