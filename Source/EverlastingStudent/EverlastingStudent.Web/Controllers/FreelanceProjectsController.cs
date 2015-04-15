using System.Web.Http.Cors;

namespace EverlastingStudent.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Http;
    using Data;
    using EverlastingStudent.Common.Infrastructure;
    using EverlastingStudent.Models.FreelanceProjects;

    [Authorize]
    public class FreelanceProjectsController : BaseApiController
    {
        public FreelanceProjectsController(IEverlastingStudentData data, IUserProvider userProvider)
            : base(data, userProvider)
        {
        }

        [Authorize]
        [HttpGet]
        [ActionName("allActive")]
        public IHttpActionResult GetAllActiveProjects()
        {
            //if (this.UserProfile.LastFreelanceProjectSearchDateTime != null && (DateTime.Now - (DateTime)this.UserProfile.LastFreelanceProjectSearchDateTime).Hours < 24)
            //{
            //    return this.BadRequest(
            //         "You can search once for 24 hours. Next search will be available after: " +
            //         string.Format("{0:hh\\:mm\\:ss}", DateTime.Now.AddHours(-24) - (DateTime)this.UserProfile.LastFreelanceProjectSearchDateTime));
            //}

            var studentBaseProject = this.UserProfile.FreelanceProjects.Select(y => y.BaseFreelanceProjectId);
            var rnd = new Random();
            int returnedRandomProjects = rnd.Next(1, this.UserProfile.Level + 1);

            // Set Serach DateTime
            this.UserProfile.LastFreelanceProjectSearchDateTime = DateTime.Now;
            this.Data.SaveChanges();

            return this.Ok(
                this.Data.BaseFreelanceProjects
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

            var project = this.Data.BaseFreelanceProjects.GetById(id);
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
                this.Data.Context.Entry(newFreelanceProject).CurrentValues.SetValues(project);

                newFreelanceProject.BaseFreelanceProject = project;
                newFreelanceProject.IsActive = true;
                newFreelanceProject.StartDateTime = DateTime.Now;
                newFreelanceProject.ProgressInPercentage = 0f;

                this.Data.SaveChanges();
                return this.Ok(project);
            }
            catch (Exception e)
            {
                return this.BadRequest(e.Message);
            }
        }

        [Authorize]
        [HttpPost]
        [ActionName("work")]
        public IHttpActionResult WorkOnProject(int id)
        {
            if (this.UserProfile == null)
            {
                return this.BadRequest("No such student found.");
            }

            var project = this.UserProfile.FreelanceProjects.FirstOrDefault(x => x.Id.Equals(id));
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



                var waitInRealMinutes = 5;
                if (project.LastWorkingDateTime != null && (DateTime.Now - (DateTime)project.LastWorkingDateTime).Minutes < waitInRealMinutes)
                {
                    this.BadRequest(
                        "Please wait Inverstor too approve your code.You can work after: " +
                        string.Format("{0:hh\\:mm\\:ss}", DateTime.Now.AddMinutes(-waitInRealMinutes) - (DateTime)project.LastWorkingDateTime));
                }

                // TODO: take user energy and gain poject progress

                this.WorkLogic(project);
                project.LastWorkingDateTime = DateTime.Now;

                // TODO: check all active projects for passed deadline
                var allProjectsOverDeadline = this.UserProfile.FreelanceProjects
                    .Where(x => x.IsActive &&
                            !x.IsSolved &&
                            !x.IsDeleted &&
                            DateTime.Now >= x.CloseForTakenDatetime);
                foreach (var freelanceProject in allProjectsOverDeadline)
                {
                    // takes 10% * User level;
                    this.UserProfile.Experience -= (long)(freelanceProject.ExperienceGain * 0.1 * this.UserProfile.Level);
                    freelanceProject.IsActive = false;
                }

                this.Data.SaveChanges();
                return this.Ok(project);
            }
            catch (Exception e)
            {
                return this.BadRequest(e.Message);
            }
        }

        private void WorkLogic(FreelanceProject project)
        {
            double expRatio;
            if (project.RequireExperience == 0)
            {
                expRatio = this.UserProfile.Experience;
            }
            else
            {
                expRatio = ((double)this.UserProfile.Experience) / project.RequireExperience;
            }

            if (expRatio <= 1.1f)
            {
                project.WorkPercentage = 20f;
            }
            else if (expRatio <= 2f)
            {
                project.WorkPercentage = 25f;
            }
            else if (expRatio <= 3f)
            {
                project.WorkPercentage = 34f;
            }
            else if (expRatio <= 5f)
            {
                project.WorkPercentage = 50f;
            }
            else
            {
                project.WorkPercentage = 100f;
            }

            if (this.UserProfile.Energy < project.EnergyCost * (project.WorkPercentage / 100.0))
            {
                throw new InvalidOperationException("Not enough energy.");
            }

            this.UserProfile.Energy -= (int)(project.EnergyCost * (project.WorkPercentage / 100.0));
            project.ProgressInPercentage += project.ProgressInPercentage + project.WorkPercentage;
            if (project.ProgressInPercentage >= 100)
            {
                this.UserProfile.Experience += project.ExperienceGain;
                this.UserProfile.Money += project.MoneyGain;
                project.IsSolved = true;
            }
        }
    }
}
