using System.Linq;

namespace EverlastingStudent.Data
{
    using System;
    using System.Collections.Generic;

    using EverlastingStudent.Common.Contracts;
    using EverlastingStudent.Data.Repositories;
    using EverlastingStudent.Data.Repositories.Contracts;
    using EverlastingStudent.Models;
    using EverlastingStudent.Models.FreelanceProjects;

    public class EverlastingStudentData : IEverlastingStudentData
    {
        private readonly IEverlastingStudentDbContext context;
        private readonly IDictionary<Type, object> repositories = new Dictionary<Type, object>();

        public EverlastingStudentData(IEverlastingStudentDbContext context)
        {
            this.context = context;
        }

        public IEverlastingStudentDbContext Context
        {
            get { return this.context; }
        }

        public IDeletableEntityRepository<Student> Students
        {
            get { return this.GetDeletableEntityRepository<Student>(); }
        }

        public IDeletableEntityRepository<Course> Courses
        {
            get { return this.GetDeletableEntityRepository<Course>(); }
        }

        public IDeletableEntityRepository<FreelanceProject> FreelanceProjects
        {
            get { return this.GetDeletableEntityRepository<FreelanceProject>(); }
        }

        public IDeletableEntityRepository<BaseFreelanceProject> BaseFreelanceProjects
        {
            get { return this.GetDeletableEntityRepository<BaseFreelanceProject>(); }
        }

        public IDeletableEntityRepository<HardwarePart> HardwareParts
        {
            get { return this.GetDeletableEntityRepository<HardwarePart>(); }
        }

        public IDeletableEntityRepository<Homework> Homeworks
        {
            get { return this.GetDeletableEntityRepository<Homework>(); }
        }

        public IGenericRepository<SpecializedCourse> SpecializedCourse
        {
            get { return this.GetRepository<SpecializedCourse>(); }
        }

        public IGenericRepository<StudentHomework> StudentHomeworks
        {
            get { return this.GetRepository<StudentHomework>(); }
        }

        public IGenericRepository<StudentInCourses> StudentInCourses
        {
            get { return this.GetRepository<StudentInCourses>(); }
        }

        public int SaveChanges()
        {
            return this.context.SaveChanges();
        }

        private IGenericRepository<T> GetRepository<T>() where T : class
        {
            if (!this.repositories.ContainsKey(typeof(T)))
            {
                var type = typeof(GenericRepository<T>);

                this.repositories.Add(typeof(T), Activator.CreateInstance(type, this.context));
            }

            return (IGenericRepository<T>)this.repositories[typeof(T)];
        }

        private IDeletableEntityRepository<T> GetDeletableEntityRepository<T>() where T : class, IDeletableEntity, new()
        {
            if (!this.repositories.ContainsKey(typeof(T)))
            {
                Type type = typeof(DeletableEntityRepository<T>);

                this.repositories.Add(typeof(T), Activator.CreateInstance(type, this.context));
            }

            return (IDeletableEntityRepository<T>)this.repositories[typeof(T)];
        }
    }
}
