namespace EverlastingStudent.Web.Controllers
{
    using System.Linq;
    using System.Web.Http;

    using EverlastingStudent.Common.Infrastructure;
    using EverlastingStudent.Data;
    using EverlastingStudent.Models;

    [Authorize]
    public class CoursesController : BaseApiController
    {
        public CoursesController(IEverlastingStudentData data, IUserProvider userProvider)
            : base(data, userProvider)
        {
        }

        [HttpGet]
        [ActionName("current")]
        public IHttpActionResult GetCurrentCourse()
        {
            var courses = this.Data.StudentInCourses.All()
                .Where(sc => sc.StudentId == this.UserProfile.Id);

            Course currentCourse = null;

            // student is not in any course - just started playing
            if (!courses.Any())
            {
                // put student in first course
                currentCourse = this.Data.Courses.All().OrderBy(c => c.Id).FirstOrDefault();
                if (currentCourse == null)
                {
                    return this.BadRequest("No courses are available.");
                }
            }

            // student has courses
            currentCourse = courses
                .Where(sc => sc.IsActive)
                .Select(sc => sc.Course)
                .FirstOrDefault();

            // sstudent should enroll in courses
            if (currentCourse == null)
            {
                return this.BadRequest("You should enroll in a course first.");
            }


            return this.Ok(new { currentCourse.Title });
        }

        [HttpGet]
        [ActionName("available")]
        public IHttpActionResult GetAvailableCourse()
        {
            var studentInCourses = this.Data.StudentInCourses.All()
                .Where(sc => sc.StudentId == this.UserProfile.Id);

            Course course;

            // student is not in any course - just started playing
            if (!studentInCourses.Any())
            {
                // return first course
                course = this.Data.Courses.All().OrderBy(c => c.Id).FirstOrDefault();

                if (course == null)
                {
                    return this.BadRequest("No courses are available.");
                }
            }
            else //student has some courses
            {
                var activeCourse = studentInCourses.FirstOrDefault(sc => sc.IsActive);

                if (activeCourse == null)
                {
                    // student has passed a course - should return the next one
                    var previousCourse = this.Data.StudentInCourses
                        .All()
                        .Where(sc => sc.StudentId == this.UserProfile.Id)
                        .OrderByDescending(sc => sc.PassedOn)
                        .Select(sc => sc.Course)
                        .FirstOrDefault();

                    course = previousCourse.NextCourse;
                }
                else // student has an active course - no courses are available
                {
                    return this.BadRequest("You have an active course. Come back when you pass the exam.");
                }
            }

            return this.Ok(new { course.Title, course.Id });
        }

        [HttpPost]
        [ActionName("enroll")]
        public IHttpActionResult EnrollInCourse(int id)
        {
            var selectedCourse = this.Data.Courses.GetById(id);
            if (selectedCourse == null)
            {
                return this.BadRequest("No such course.");
            }

            var activeCourses = this.Data.StudentInCourses.All()
                .Where(sc => sc.IsActive && sc.StudentId == this.UserProfile.Id);
            if (activeCourses.Any())
            {
                return this.BadRequest("Cannot enroll in a new course until you pass the current course.");
            }

            var pastCourses = this.Data.StudentInCourses.All()
                .Where(sc => sc.StudentId == this.UserProfile.Id);
            int? validId;
            if (!pastCourses.Any())
            {
                // if student has no past courses
                // should enroll in first course
                validId = this.Data.Courses.All()
                    .OrderBy(c => c.Id)
                    .Select(c => c.Id)
                    .FirstOrDefault();
            }
            else
            {
                // if student has past courses
                // should enroll in next course
                validId = pastCourses
                    .OrderByDescending(sc => sc.PassedOn)
                    .Select(sc => sc.Course.NextCourseId)
                    .FirstOrDefault();
            }

            if (id != validId)
            {
                return this.BadRequest("Cannot enroll in this course.");
            }

            // enroll student
            this.Data.StudentInCourses.Add(new StudentInCourses()
            {
                Student = this.UserProfile,
                Course = selectedCourse,
                IsActive = true
            });
            this.Data.SaveChanges();

            return this.Ok();
        }
    }
}
