namespace EverlastingStudent.Data
{
    using EverlastingStudent.Data.Repositories.Contracts;
    using EverlastingStudent.Models;
    using EverlastingStudent.Models.FreelanceProjects;

    public interface IEverlastingStudentData
    {
        IEverlastingStudentDbContext Context { get; }

        IDeletableEntityRepository<Student> Students { get; }

        IDeletableEntityRepository<Course> Courses { get; }

        IDeletableEntityRepository<FreelanceProject> FreelanceProjects { get; }

        IDeletableEntityRepository<BaseFreelanceProject> BaseFreelanceProjects { get; }

        IDeletableEntityRepository<HardwarePart> HardwareParts { get; }

        IDeletableEntityRepository<Drink> Drinks { get; }

        IDeletableEntityRepository<Homework> Homeworks { get; }

        IDeletableEntityRepository<Lecture> Lectures { get; }

        IGenericRepository<SpecializedCourse> SpecializedCourse { get; }

        IGenericRepository<StudentHomework> StudentHomeworks { get; }

        IGenericRepository<StudentCourses> StudentCourses { get; }

        IGenericRepository<StudentLectures> StudentLectures { get; }

        int SaveChanges();
    }
}
