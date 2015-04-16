namespace EverlastingStudent.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;

    using EverlastingStudent.Common.Infrastructure;
    using EverlastingStudent.Data;

    public class ExamsController : BaseApiController
    {
        public ExamsController(IEverlastingStudentData data, IUserProvider userProvider)
            : base(data, userProvider)
        {
        }

        [Authorize]
        [HttpGet]
        [ActionName("available")]
        public IHttpActionResult GetAvailableExam()
        {
            var currentCourse  = this.UserProfile.StudentCourses
                .Where(sc => sc.IsActive)
                .Select(sc => sc.Course)
                .FirstOrDefault();

            if (currentCourse == null)
            {
                return this.BadRequest("You should enroll in a course first.");
            }

            var exam = currentCourse.Exam;

            return this.Ok(new
            {
                exam.ExamDurationInMinutes,
                exam.RequireExpForExam,
                exam.Id
            });
        }

        [Authorize]
        [HttpPost]
        [ActionName("take")]
        public IHttpActionResult TakeExam(int id)
        {
            var student = this.Data.Students.GetById(this.UserProfile.Id);
            if (student.IsBusy)
            {
                if (!this.CheckIfAnyActionIsDone(student))
                {
                    return this.BadRequest("You are buzy right now.");
                }
            }

            var currentStudentCourse = student.StudentCourses
                .Where(sc => sc.IsActive)
                .FirstOrDefault();

            if (currentStudentCourse == null)
            {
                return this.BadRequest("You should enroll in a course first.");
            }

            if (currentStudentCourse.Course.Exam.Id != id)
            {
                return this.BadRequest("You cannot take this exam now.");
            }

            var exam = currentStudentCourse.Course.Exam;

            if (student.Experience < exam.RequireExpForExam)
            {
                return this.BadRequest(string.Format("You need {0} experience to take this exam. You now have only {1} XP. Do some homeworks or projects to increase your XP.", 
                    exam.RequireExpForExam,
                    student.Experience));
            }
            
            // take exam
            student.IsBusy = true;
            student.BusyUntil = DateTime.Now.AddMinutes(exam.ExamDurationInMinutes);

            currentStudentCourse.ExamInProgress = true;

            this.Data.StudentCourses.Update(currentStudentCourse);
            this.Data.Students.Update(student);
            this.Data.SaveChanges();

            return this.Ok(string.Format(
                "You are taking the exam for the course {0}. You will finish after {1} minutes.",
                currentStudentCourse.Course.Title,
                exam.ExamDurationInMinutes));
        }
    }
}
