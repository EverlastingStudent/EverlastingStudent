using System.Web.Http.Cors;

namespace EverlastingStudent.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Http;
    using System.Web.UI.WebControls;

    using EverlastingStudent.Common.Infrastructure;
    using EverlastingStudent.Common.Models;
    using EverlastingStudent.Data;
    using EverlastingStudent.Models;

    public class BaseApiController : ApiController
    {
        private IUserProvider provider;
        private static Random random = new Random();

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

        protected bool CheckIfAnyActionIsDone(Student student)
        {

            if (student.BusyUntil > DateTime.Now)
            {
                return false;
            }

            this.GiveStats(student);
            return true;

        }

        protected void GiveEnergy(Student student)
        {
            var minutesDifference = (int)(DateTime.Now.Subtract(student.LastAction).TotalMinutes * student.CoefficientEnergyGain) ;
            student.LastAction = DateTime.Now;
            if (student.Energy + minutesDifference > 100)
            {
                student.Energy = 100;
                return;
            }

            student.Energy += minutesDifference;
        }

        private void GiveStats(Student student)
        {
            var homework = student.StudentHomeworks.FirstOrDefault(x => x.InProgress);

            if (homework != null)
            {
                student.IsBusy = false;
                homework.InProgress = false;
                // calculate chance for homework
                var randomItemNumber = random.NextDouble();

                if (homework.Type == TypeOfDifficulty.Easy)
                {
                    if (random.NextDouble() < 0.90)
                    {
                        this.GiveHomeworkStats(homework, student);
                        return;
                    }

                    return;
                }

                if (homework.Type == TypeOfDifficulty.Medium)
                {
                    if (random.NextDouble() < 0.70)
                    {
                        this.GiveHomeworkStats(homework, student);
                        return;
                    }

                    return;
                }

                if (random.NextDouble() < 0.40)
                {
                    this.GiveHomeworkStats(homework, student);
                }
            }

            // add check if other action is inprograss
        }

        private void GiveHomeworkStats(StudentHomework homework, Student student)
        {
            student.Knowledge += (long)(homework.KnowledgeGain * student.CoefficientKnowledgeGain);
            student.Experience += (long)(homework.ExperienceGain * student.CoefficientExperienceGain);
            homework.IsSolved = true;
        }
    }
}