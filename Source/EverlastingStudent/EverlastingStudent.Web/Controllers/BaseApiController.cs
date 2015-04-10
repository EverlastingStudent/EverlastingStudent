namespace EverlastingStudent.Web.Controllers
{
    using System.Web.Http;

    using EverlastingStudent.Common.Infrastructure;
    using EverlastingStudent.Data;
    using EverlastingStudent.Models;

    public class BaseApiController : ApiController
    {
        private IUserProvider provider;

        public BaseApiController(IEverlastingStudentData data, IUserProvider userProvider)
        {
            this.Data = data;
            this.provider = userProvider;
        }

        protected IEverlastingStudentData Data { get; set; }

        protected Student UserProfile
        {
            get
            {
                return this.Data.Students.GetById(this.provider.GetUserId());
            }
        }
    }
}