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

    public class StudentsController : BaseApiController
    {
        public StudentsController(IEverlastingStudentData data, IUserProvider userProvider)
            : base(data, userProvider)
        {
        }

        [HttpGet]
        [ActionName("stats")]
        [Authorize]
        public IHttpActionResult GetStudentStats()
        {
            var student = this.Data.Students.GetById(this.UserProfile.Id);

            this.CheckIfAnyActionIsDone(student);

            return this.Ok(new
            {
                student.BusyUntil,
                student.Energy,
                student.Experience,
                student.Knowledge,
                student.Level,
                student.Money
            });
        }
    }
}
