namespace EverlastingStudent.Data
{
    using EverlastingStudent.Data.Repositories.Contracts;
    using EverlastingStudent.Models;

    public interface IEverlastingStudentData
    {
        IEverlastingStudentDbContext Context { get; }

        IDeletableEntityRepository<Student> Students { get; }

        IDeletableEntityRepository<Course> Courses { get; }

        IDeletableEntityRepository<FreelanceProject> FreelanceProjects { get; }

        IDeletableEntityRepository<HardwarePart> HardwareParts { get; }

        IDeletableEntityRepository<Homework> Homeworks { get; }

        IGenericRepository<SpecializedCourse> SpecializedCourse { get; }

        IGenericRepository<StudentFreelanceProject> StudentFreelanceProjects { get; }

        IGenericRepository<StudentHomework> StudentHomeworks { get; }

        IGenericRepository<StudentInCourses> StudentInCourses { get; }
    }
}
