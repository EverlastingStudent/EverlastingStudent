namespace EverlastingStudent.Data
{
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;

    using EverlastingStudent.Models;

    public interface IEverlastingStudentDbContext
    {

        IDbSet<Course> Courses { get; set; }

        IDbSet<Difficulty> Difficulties { get; set; }

        IDbSet<Drink> Drinks { get; set; }

        IDbSet<Exam> Exams { get; set; }

        IDbSet<FreelanceProject> FreelanceProjects { get; set; }

        IDbSet<HardwarePart> HardwareParts { get; set; }

        IDbSet<Homework> Homeworks { get; set; }

        IDbSet<SpecializedCourse> SpecializedCourses { get; set; }

        IDbSet<StudentFreelanceProject> StudentFreelanceProjects { get; set; }

        IDbSet<StudentHomework> StudentHomeworks { get; set; }

        IDbSet<TEntity> Set<TEntity>() where TEntity : class;

        DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;

        int SaveChanges();
    }
}
