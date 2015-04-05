﻿namespace EverlastingStudent.Models
{
    using System;
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using EverlastingStudent.Common;

    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    public class Student : IdentityUser
    {
        private ICollection<StudentHomework> studentHomeworks;
        private ICollection<StudentFreelanceProject> studentFreelanceProjects;
        private ICollection<Course> courses;
        private ICollection<HardwarePart> hardwareParts; 

        public Student()
        {
            this.studentHomeworks = new HashSet<StudentHomework>();
            this.studentFreelanceProjects = new HashSet<StudentFreelanceProject>();
            this.courses = new HashSet<Course>();
            this.hardwareParts = new HashSet<HardwarePart>();
        }

        public long Experience { get; set; }

        public long Knowledge { get; set; }

        public long Money { get; set; }

        public int Energy { get; set; }

        public DateTime LastAction { get; set; }

        public PlayerType PlayerType { get; set; }

        public virtual ICollection<StudentHomework> StudentHomeworks
        {
            get { return this.studentHomeworks; }
            set { this.studentHomeworks = value; }
        }

        public virtual ICollection<StudentFreelanceProject> StudentFreelanceProjects
        {
            get { return this.studentFreelanceProjects; }
            set { this.studentFreelanceProjects = value; }
        }

        public virtual ICollection<Course> Courses
        {
            get { return this.courses; }
            set { this.courses = value; }
        }

        public virtual ICollection<HardwarePart> HardwareParts
        {
            get { return this.hardwareParts; }
            set { this.hardwareParts = value; }
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<Student> manager,
            string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }
    }
}
