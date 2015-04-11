namespace EverlastingStudent.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using EverlastingStudent.Common.Contracts;
    using EverlastingStudent.Common.Models;
    using EverlastingStudent.Models.FreelanceProjects;

    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    public class Student : IdentityUser, IDeletableEntity
    {
        private ICollection<StudentHomework> studentHomeworks;
        private ICollection<FreelanceProject> studentFreelanceProjects;
        private ICollection<StudentInCourses> studentInCourses;
        private ICollection<HardwarePart> hardwareParts;

        private long experience;

        public Student()
        {
            this.studentHomeworks = new HashSet<StudentHomework>();
            this.studentFreelanceProjects = new HashSet<FreelanceProject>();
            this.studentInCourses = new HashSet<StudentInCourses>();
            this.hardwareParts = new HashSet<HardwarePart>();
        }

        public long Experience
        {
            get
            {
                return this.experience;
            }

            set
            {
                this.experience = value;
                if (this.experience < 0)
                {
                    this.Level = 0;
                }
                else
                {
                    // exp = 10 -> 1 level, exp = 40-> 2 level, exp = 90-> 3level ...
                    this.Level = (int)Math.Sqrt(this.experience / 10.0);
                }
            }
        }

        public int Level { get; private set; }

        public long Knowledge { get; set; }

        public long Money { get; set; }

        public int Energy { get; set; }

        public DateTime? LastAction { get; set; }

        public float ChanceToSolveCoefficient { get; set; }

        public float CoefficientKnowledgeGain { get; set; }

        public float CoefficientEnergyGain { get; set; }

        public float CoefficientExperienceGain { get; set; }

        public float CoefficientMoneyGain { get; set; }

        public float CoefficientEnergyLoss { get; set; }

        public float CoefficientMoneyLoss { get; set; }

        public bool IsBusy { get; set; }

        public DateTime? BusyUntil { get; set; }

        public PlayerType PlayerType { get; set; }

        public DateTime? LastFreelanceProjectSearchDateTime { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public virtual ICollection<StudentHomework> StudentHomeworks
        {
            get { return this.studentHomeworks; }
            set { this.studentHomeworks = value; }
        }

        public virtual ICollection<FreelanceProject> FreelanceProjects
        {
            get { return this.studentFreelanceProjects; }
            set { this.studentFreelanceProjects = value; }
        }

        public virtual ICollection<StudentInCourses> StudentInCourses
        {
            get { return this.studentInCourses; }
            set { this.studentInCourses = value; }
        }

        public virtual ICollection<HardwarePart> HardwareParts
        {
            get { return this.hardwareParts; }
            set { this.hardwareParts = value; }
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<Student> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);

            // Add custom user claims here
            return userIdentity;
        }
    }
}
