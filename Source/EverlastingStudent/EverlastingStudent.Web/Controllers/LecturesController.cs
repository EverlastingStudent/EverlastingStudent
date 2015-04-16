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
    using EverlastingStudent.Models;

    public class LecturesController : BaseApiController
    {
        public LecturesController(IEverlastingStudentData data, IUserProvider userProvider)
            : base(data, userProvider)
        {
        }

        [Authorize]
        [HttpGet]
        [ActionName("available")]
        public IHttpActionResult GetAvailableLectures()
        {
            //get current course
            var currentCourse = this.UserProfile.StudentCourses
                .Where(sc => sc.IsActive)
                .Select(sc => sc.Course)
                .FirstOrDefault();

            if (currentCourse == null)
            {
                return this.BadRequest("You should enroll in a course first.");
            }

            //get all lectures from this course
            var lectures = currentCourse.Lectures
                .Select(l => new
                {
                    l.Id,
                    l.Title,
                    l.DurationInMinutes,
                    l.CoefficientKnowledgeGain,
                });

            if (lectures == null)
            {
                return this.BadRequest("Sorry, there are no lectures for this course.");
            }

            //determine if user has already attended some of them and select only these that they have not attended
            var available = lectures
                .Where(l => !this.Data.StudentLectures
                    .All()
                    .Where(sl => sl.StudentId == this.UserProfile.Id) // get all StudentLectures for the user
                    .Where(sl => sl.IsPassed || sl.IsActive) // may be useless // get SL that he has not passed or are not active
                    .Select(sl => sl.LectureId) // select lectures that user has attended
                    .Contains(l.Id)); // determine if the lecture from the course is among these lectures

            if (available == null)
            {
                return this.BadRequest("There are no more available lectures.");
            }

            return this.Ok(available);
        }

        [Authorize]
        [HttpPost]
        [ActionName("attend")]
        public IHttpActionResult AttendLecture(int id)
        {
            var lecture = this.Data.Lectures.GetById(id);
            if (lecture == null)
            {
                return this.BadRequest("No such lecture.");
            }

            var student = this.Data.Students.GetById(this.UserProfile.Id);
            if (student.IsBusy)
            {
                if (!this.CheckIfAnyActionIsDone(student))
                {
                    return this.BadRequest("You are buzy right now.");
                }
            }

            var currentCourse = this.UserProfile.StudentCourses
                .Where(sc => sc.IsActive)
                .Select(sc => sc.Course)
                .FirstOrDefault();

            if (currentCourse == null)
            {
                return this.BadRequest("You should enroll in a course first.");
            }

            var lectureIds = currentCourse.Lectures
                .Select(l => l.Id);

            if (lectureIds == null)
            {
                return this.BadRequest("You cannot attend this lecture. There are no lectures for your current course.");
            }

            var availableLecturesIds = lectureIds
                .Where(l => !this.Data.StudentLectures
                    .All()
                    .Where(sl => sl.StudentId == this.UserProfile.Id) // get all StudentLectures for the user
                    .Where(sl => sl.IsPassed || sl.IsActive) // may be useless // get SL that he has not passed or are not active
                    .Select(sl => sl.LectureId) // select lectures that user has attended
                    .Contains(l)); // determine if the lecture from the course is among these lectures

            if (!availableLecturesIds.Contains(id))
            {
                return this.BadRequest("You cannot attend this lecture.");
            }

            // attend lecture
            student.IsBusy = true;
            student.BusyUntil = DateTime.Now.AddMinutes(lecture.DurationInMinutes);

            var studentLecture = new StudentLectures()
            {
                Lecture = lecture,
                Student = student,
                IsActive = true
            };

            this.Data.StudentLectures.Add(studentLecture);
            this.Data.Students.Update(student);
            this.Data.SaveChanges();

            return this.Ok(string.Format(
                "You are attending the lecture {0}. You will finish after {1} minutes.",
                lecture.Title,
                lecture.DurationInMinutes));
        }
    }
}
