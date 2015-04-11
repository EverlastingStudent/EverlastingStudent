namespace EverlastingStudent.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Http;

    using AutoMapper.QueryableExtensions;

    using EverlastingStudent.Common.Infrastructure;
    using EverlastingStudent.Common.Models;
    using EverlastingStudent.Data;
    using EverlastingStudent.DataTransferObjects;
    using EverlastingStudent.Models;

    [Authorize]
    public class HomeworksController : BaseApiController
    {
        public HomeworksController(IEverlastingStudentData data, IUserProvider userProvider)
            : base(data, userProvider)
        {
        }

        // GET: Homeworks
        [HttpGet]
        public IHttpActionResult GetHomeworks()
        {

            var unsolvedHomeworks = this.Data.StudentHomeworks.All().Where(sh => !sh.IsSolved && sh.StudentId == this.UserProfile.Id);
            if (unsolvedHomeworks.Any())
            {
                foreach (var homework in unsolvedHomeworks)
                {
                    this.Data.StudentHomeworks.Delete(homework);
                }
            }

            var easyHomework = this.Data.Homeworks
                                .All()
                                .Where(x => x.Type == TypeOfDifficulty.Easy)
                                .OrderBy(x => Guid.NewGuid())
                               .FirstOrDefault();

            var mediumHomework = this.Data.Homeworks
                               .All()
                               .Where(x => x.Type == TypeOfDifficulty.Medium)
                               .OrderBy(x => Guid.NewGuid())
                               .FirstOrDefault();

            var hardHomework = this.Data.Homeworks
                               .All()
                               .Where(x => x.Type == TypeOfDifficulty.Hard)
                               .OrderBy(x => Guid.NewGuid())
                               .FirstOrDefault();

            this.CalculateStats(easyHomework);
            this.CalculateStats(mediumHomework);
            this.CalculateStats(hardHomework);

            this.Data.SaveChanges();

            var homeworkskDto = this.Data.StudentHomeworks
                .All()
                .OrderByDescending(x => x.CreatedOn)
                .Take(3)
                .Project()
                .To<GetHomeworksDto>()
                .ToList();

            return this.Ok(homeworkskDto);
        }

        [HttpPost]
        public IHttpActionResult Solve(int id)
        {
            var homework = this.Data.StudentHomeworks.All().FirstOrDefault(x => x.Id == id);

            if (homework == null)
            {
                return this.BadRequest("Homework doesn't exist");
            }

            if (homework.StudentId != this.UserProfile.Id)
            {
                return this.BadRequest("Homework doesn't belong to you");
            }

            if (homework.IsSolved)
            {
                return this.BadRequest("You already solved this homework");
            }

            var student = this.Data.Students.GetById(this.UserProfile.Id);
            //student.BusyUntil = DateTime.Now;
            //this.Data.SaveChanges();
            //return null;

            if (student.IsBusy)
            {
                if (student.BusyUntil > DateTime.Now)
                {
                    return this.BadRequest("You are buzy right now");
                }

                this.GiveStats();
                this.Data.SaveChanges();
            }

            
            if (student.Energy >= homework.EnergyCost)
            {
                student.IsBusy = true;
                student.BusyUntil = DateTime.Now.AddMinutes(homework.SolveDurabationInMinutes);
                student.Energy -= homework.EnergyCost;
                homework.InProgress = true;
                this.Data.SaveChanges();
                return this.Ok("Solving is in progress... " + homework.SolveDurabationInMinutes + " minutes left");
            }
            else
            {
                return this.BadRequest("You don't have enough energy to solve this homework. Drink Some Coffee or take a rest"); 
            }
        }

        private void CalculateStats(Homework homework)
        {
            var homeworkKnowledgeGain = this.UserProfile.Knowledge * 0.01;
            var homeworkExperienceGain = this.UserProfile.Experience * 0.01;
            var chanceToSolve = 0.0f;
            const int energyCost = 30; // add constant

            if (homework.Type == TypeOfDifficulty.Easy)
            {
                chanceToSolve = 0.90f;
            }
            else
            {
                chanceToSolve = homework.Type == TypeOfDifficulty.Medium ? 0.70f : 0.40f;
            }

            var studentHomework = new StudentHomework
            {
                StudentId = this.UserProfile.Id,
                Homework = homework,
                EnergyCost = energyCost,
                ExperienceGain = homeworkExperienceGain < 1 ? 1 : homeworkExperienceGain,
                KnowledgeGain = homeworkKnowledgeGain < 1 ? 1 : homeworkKnowledgeGain,
                SolveDurabationInMinutes = 10,
                Type = homework.Type,
                CreatedOn = DateTime.Now
            };

            this.Data.StudentHomeworks.Add(studentHomework);
        }
    }
}