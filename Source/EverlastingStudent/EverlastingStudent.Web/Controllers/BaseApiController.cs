namespace EverlastingStudent.Web.Controllers
{
    using System.Web.Http;
    using System.Web.Http.Controllers;

    using EverlastingStudent.Data;
    using EverlastingStudent.Models;

    public class BaseApiController : ApiController
    {
        public BaseApiController(IEverlastingStudentData data)
        {
            this.Data = data;
            this.UserProfile = this.User;
        }

        protected IEverlastingStudentData Data { get; set; }

        protected Student UserProfile { get; set; }

        
    }
}