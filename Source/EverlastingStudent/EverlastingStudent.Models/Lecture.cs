using EverlastingStudent.Common.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EverlastingStudent.Models
{
    public class Lecture : DeletableEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public int CourseId { get; set; }

        public Course Course { get; set; }

        public int DurationInMinutes { get; set; }

        public double CoefficientKnowledgeGain { get; set; }

        public int? NextLectureId { get; set; }

        public Lecture NextLecture { get; set; }
    }
}
