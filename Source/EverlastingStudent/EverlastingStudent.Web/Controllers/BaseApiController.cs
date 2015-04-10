namespace EverlastingStudent.Web.Controllers
{
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.Controllers;
    using System.Web.Http.Results;

    using EverlastingStudent.Common.Infrastructure;
    using EverlastingStudent.Data;
    using EverlastingStudent.Models;

    using Microsoft.AspNet.Identity;

    public class BaseApiController : ApiController
    {
        public BaseApiController(IEverlastingStudentData data, IUserProvider userProvider)
        {
            this.Data = data;
            this.UserProfile = this.Data.Students.GetById(userProvider.GetUserId());
        }

        protected IEverlastingStudentData Data { get; set; }

        protected Student UserProfile { get; set; }
    }
}