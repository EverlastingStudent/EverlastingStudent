namespace EverlastingStudent.Data
{
    using System.Data.Entity;

    using EverlastingStudent.Models;
    using EverlastingStudent.Models.FreelanceProjects;

    using Microsoft.AspNet.Identity.EntityFramework;

    public class EverlastingStudentDbContext : IdentityDbContext<Student>, IEverlastingStudentDbContext
    {
        public EverlastingStudentDbContext()
            : base("EverlastingStudent", throwIfV1Schema: false)
        {
             Database.SetInitializer(new MigrateDatabaseToLatestVersion<EverlastingStudentDbContext, Migrations.DefaultMigrationConfiguration>());
        }

        public IDbSet<Course> Courses { get; set; }

        public IDbSet<Drink> Drinks { get; set; }

        public IDbSet<Exam> Exams { get; set; }

        public IDbSet<FreelanceProject> FreelanceProjects { get; set; }

        public IDbSet<BaseFreelanceProject> BaseFreelanceProjects { get; set; }

        public IDbSet<HardwarePart> HardwareParts { get; set; }

        public IDbSet<Homework> Homeworks { get; set; }

        public IDbSet<Lecture> Lectures { get; set; }

        public IDbSet<SpecializedCourse> SpecializedCourses { get; set; }

        public IDbSet<StudentHomework> StudentHomeworks { get; set; }

        public IDbSet<StudentLectures> StudentLectures { get; set; }

        public IDbSet<StudentCourses> StudentCourses { get; set; }

        public static EverlastingStudentDbContext Create()
        {
            return new EverlastingStudentDbContext();
        }

        public new IDbSet<T> Set<T>() where T : class
        {
            return base.Set<T>();
        }
    }
}
