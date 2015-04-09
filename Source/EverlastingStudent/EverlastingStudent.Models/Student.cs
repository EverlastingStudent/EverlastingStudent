﻿namespace EverlastingStudent.Models
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

        public Student()
        {
            this.studentHomeworks = new HashSet<StudentHomework>();
            this.studentFreelanceProjects = new HashSet<FreelanceProject>();
            this.studentInCourses = new HashSet<StudentInCourses>();
            this.hardwareParts = new HashSet<HardwarePart>();
        }

        public long Experience { get; set; }

        public long Knowledge { get; set; }

        public long Money { get; set; }

        public int Energy { get; set; }

        public DateTime? LastAction { get; set; }

        public float CoefficientKnowledgeGain { get; set; }

        public float CoefficientEnergyGain { get; set; }

        public float CoefficientExperienceGain { get; set; }

        public float CoefficientMoneyGain { get; set; }

        public float CoefficientEnergyLoss { get; set; }

        public float CoefficientMoneyLoss { get; set; }

        public bool IsBusy { get; set; }

        public PlayerType PlayerType { get; set; }

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

        public bool TakeFreelanceProject(BaseFreelanceProject baseFreelanceProject)
        {
            if (!baseFreelanceProject.IsActive)
            {
                throw new InvalidOperationException("Project is not active.");
            }

            if (this.FreelanceProjects.Any(x => x.BaseFreelanceProjectId == baseFreelanceProject.Id))
            {
                throw new InvalidOperationException("You are working on this project.");
            }

            if (this.Experience < baseFreelanceProject.RequireExperience)
            {
                throw new InvalidOperationException("Not enough experience.");
            }

            // Copy properties to newFrelanceProject
            var newFreelanceProject = FreelanceProject.CopyToFreelanceProject(baseFreelanceProject);

            if (newFreelanceProject != null)
            {
                newFreelanceProject.BaseFreelanceProject = baseFreelanceProject;
                newFreelanceProject.IsActive = true;
                newFreelanceProject.ActivatedDateTime = DateTime.Now;
                newFreelanceProject.ProgressInPercentage = 0f;
                this.FreelanceProjects.Add(newFreelanceProject);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
