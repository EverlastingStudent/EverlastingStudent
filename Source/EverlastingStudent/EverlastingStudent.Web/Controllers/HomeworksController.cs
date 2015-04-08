namespace EverlastingStudent.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;

    using EverlastingStudent.Data;
    using EverlastingStudent.Web.Models;

    public class HomeworksController : BaseApiController
    {
        public HomeworksController(IEverlastingStudentData data) : base(data)
        {
        }

        // GET: Homeworks
        public IEnumerable<HomeworksDto> GetHomeworks(int type)
        {

            
        }
    }
}