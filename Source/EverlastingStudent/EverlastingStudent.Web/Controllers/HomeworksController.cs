namespace EverlastingStudent.Web.Controllers
{
    using System;
    using System.Data.Entity.SqlServer;
    using System.Linq;
    using System.Web.Http;

    using AutoMapper.QueryableExtensions;

    using EverlastingStudent.Common.Infrastructure;
    using EverlastingStudent.Common.Models;
    using EverlastingStudent.Data;
    using EverlastingStudent.DataTransferObjects;
    using EverlastingStudent.Models;

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

            var easyHomeworkDto = this.Data.StudentHomeworks
                .All()
                .OrderByDescending(x => x.CreatedOn)
                .Take(3)
                .Project()
                .To<GetHomeworksDto>()
                .ToList();

            return this.Ok(easyHomeworkDto);
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
                CreatedOn = DateTime.Now
            };

            this.Data.StudentHomeworks.Add(studentHomework);
        }
    }
}