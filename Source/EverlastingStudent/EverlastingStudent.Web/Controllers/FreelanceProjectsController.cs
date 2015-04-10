namespace EverlastingStudent.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Http;
    using EverlastingStudent.Data;
    using EverlastingStudent.Models.FreelanceProjects;
    using Microsoft.AspNet.Identity;
    using EverlastingStudent.Common.Infrastructure;

    [Authorize]
    public class FreelanceProjectsController : BaseApiController
    {
        private IEverlastingStudentData data;
        public FreelanceProjectsController(IEverlastingStudentData data, IUserProvider userProvider)
            : base(data, userProvider)
        {
            this.data = data;
        }

        [Authorize]
        [HttpGet]
        [ActionName("allActive")]
        public IHttpActionResult GetAllActiveProjects()
        {
            if (this.UserProfile.LastFreelanceProjectSearchDateTime != null && (DateTime.Now - (DateTime)this.UserProfile.LastFreelanceProjectSearchDateTime).Hours < 24)
            {
                this.BadRequest(
                    "You can search once for 24 hours. Next search will be available after: " +
                    string.Format("{0:hh\\:mm\\:ss}", DateTime.Now - (DateTime)this.UserProfile.LastFreelanceProjectSearchDateTime));
            }

            var studentBaseProject = this.UserProfile.FreelanceProjects.Select(y => y.BaseFreelanceProjectId);
            var rnd = new Random();
            int returnedRandomProjects = rnd.Next(1, this.UserProfile.Level + 1);

            return this.Ok(
                this.data.BaseFreelanceProjects
                .All()
                .Where(x => !(x is FreelanceProject) && x.IsActive && studentBaseProject.All(z => z != x.Id))
                .Take(returnedRandomProjects)
                .OrderByDescending(x => x.CloseForTakenDatetime)
                .ToList());

            //.Select(x => new
            //{
            //    x.Id,
            //    x.IsActive,
            //    x.Title,
            //    x.Content,
            //    x.EnergyCost,
            //    x.RequireExperience,
            //    x.ExperienceGain,
            //    x.MoneyGain,
            //    x.OpenForTakenDatetime,
            //    x.SolveDurabationInHours,
            //    x.CloseForTakenDatetime,
            //})
        }

        [Authorize]
        [HttpGet]
        [ActionName("myProjects")]
        public IHttpActionResult GetUsersProjects()
        {
            return this.Ok(this.UserProfile.FreelanceProjects.ToList());
            //.Select(x => new
            //{
            //    x.Id,
            //    x.StartDateTime,
            //    x.StudentId,
            //    x.IsSolved,
            //    x.ProgressInPercentage,
            //    x.BaseFreelanceProject,
            //    x.BaseFreelanceProjectId,
            //})
        }

        [Authorize]
        [HttpPost]
        [ActionName("take")]
        public IHttpActionResult TakeProject(int id)
        {
            if (this.UserProfile == null)
            {
                return this.BadRequest("No such student");
            }

            var project = this.data.BaseFreelanceProjects.GetById(id);
            if (project == null)
            {
                return this.BadRequest("No such project");
            }

            if (project.GetType() != typeof(BaseFreelanceProject))
            {
                return this.BadRequest("You cannot choose this type of project!");
            }

            try
            {
                if (!project.IsActive)
                {
                    throw new InvalidOperationException("Project is not active.");
                }

                if (this.UserProfile.FreelanceProjects.Any(x => x.BaseFreelanceProjectId == project.Id))
                {
                    throw new InvalidOperationException("You are working on this project.");
                }

                if (this.UserProfile.Experience < project.RequireExperience)
                {
                    throw new InvalidOperationException("Not enough experience.");
                }

                var newFreelanceProject = new FreelanceProject();
                this.UserProfile.FreelanceProjects.Add(newFreelanceProject);

                // Copy properties to newFrelanceProject
                this.data.Context.Entry(newFreelanceProject).CurrentValues.SetValues(project);

                newFreelanceProject.BaseFreelanceProject = project;
                newFreelanceProject.IsActive = true;
                newFreelanceProject.StartDateTime = DateTime.Now;
                newFreelanceProject.ProgressInPercentage = 0f;

                this.data.SaveChanges();
                return this.Ok();
            }
            catch (Exception e)
            {
                return this.BadRequest(e.Message);
            }
        }

        [Authorize]
        [HttpPost]
        [ActionName("work")]
        public IHttpActionResult WorkOnProject(object id)
        {
            if (this.UserProfile == null)
            {
                return this.BadRequest("No such student found.");
            }

            var project = this.data.FreelanceProjects.GetById(id);
            if (project == null)
            {
                return this.BadRequest("No such project found.");
            }

            //if (!(project is FreelanceProject))
            //{
            //    return this.BadRequest("No such project found.");
            //}

            try
            {
                if (this.UserProfile.FreelanceProjects.All(x => x.Id != project.Id))
                {
                    throw new InvalidOperationException("You have to add project to work on it.");
                }

                if (project.IsDeleted)
                {
                    throw new NullReferenceException("Can't work on deleted project.");
                }

                if (project.IsSolved)
                {
                    throw new InvalidOperationException("Project is done.");
                }

                if (project.CloseForTakenDatetime <= DateTime.Now)
                {
                    throw new InvalidOperationException("Project is no longer active.");
                }

                if (project.OpenForTakenDatetime > DateTime.Now)
                {
                    throw new InvalidOperationException("Project is not active yet.");
                }

                if (this.UserProfile.Experience < project.RequireExperience)
                {
                    throw new InvalidOperationException("Not enough experience.");
                }

                if (this.UserProfile.Energy < project.EnergyCost)
                {
                    throw new InvalidOperationException("Not enough energy.");
                }


                // TODO: take user energy and gain poject progress
                // TODO: check all active projects for passed deadline
                return this.Ok(project);
            }
            catch (Exception e)
            {
                return this.BadRequest(e.Message);
            }
        }
    }
}
