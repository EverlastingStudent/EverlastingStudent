namespace EverlastingStudent.Common.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using EverlastingStudent.Common.Contracts;

    public abstract class DeletableEntity : IDeletableEntity
    {
        [Editable(false)]
        public bool IsDeleted { get; set; }

        [Editable(false)]
        public DateTime? DeletedOn { get; set; }
    }
}
