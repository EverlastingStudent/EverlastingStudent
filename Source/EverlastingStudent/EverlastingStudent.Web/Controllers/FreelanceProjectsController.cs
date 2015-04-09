namespace EverlastingStudent.Web.Controllers
{
    using System.Linq;
    using System.Web.Http;
    using EverlastingStudent.Data;
    using Microsoft.AspNet.Identity;

    public class FreelanceProjectsController : BaseApiController
    {
        private IEverlastingStudentData data;

        public FreelanceProjectsController(IEverlastingStudentData data)
            : base(data)
        {
            this.data = data;
        }

        [HttpGet]
        [ActionName("all")]
        public IHttpActionResult GetAllProjects()
        {
            return this.Ok(this.data.FreelanceProjects.All().Where(x => x.IsActive));
        }

        [HttpGet]
        [ActionName("myProjects")]
        public IHttpActionResult GetUsersProjects(object studentId)
        {
            var student = this.data.Students.GetById(studentId);
            return this.Ok(this.data.FreelanceProjects.All().Where(x => x.StudentId.Equals(studentId)));
        }

        [Authorize]
        [HttpPost]
        [ActionName("take")]
        public IHttpActionResult TakeProject(int projectId)
        {
            var currentUserId = User.Identity.GetUserId();
            var student = this.data.Students.All().FirstOrDefault(x => x.Id == currentUserId);

            // var student = this.data.Students.GetById(studentId);
            if (student == null)
            {
                return this.BadRequest("No such student");
            }

            var project = this.data.BaseFreelanceProjects.GetById(projectId);
            if (project == null)
            {
                return this.BadRequest("No such project");
            }
            
            student.TakeFreelanceProject(project);
            this.data.SaveChanges();

            return this.Ok();
        }

        [HttpPost]
        [ActionName("work")]
        public IHttpActionResult WorkOnProject(object studentId, object projectId)
        {
            var student = this.data.Students.GetById(studentId);
            if (student == null)
            {
                return this.BadRequest("No such student found.");
            }

            var project = this.data.FreelanceProjects.GetById(projectId);
            if (project == null)
            {
                return this.BadRequest("No such project found.");
            }

            project.Do();

            return this.Ok(project);
        }
    }
}
