namespace EverlastingStudent.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Http;

    using EverlastingStudent.Common.Infrastructure;
    using EverlastingStudent.Data;

    [Authorize]
    public class HardwaresController : BaseApiController
    {
        public HardwaresController(IEverlastingStudentData data, IUserProvider userProvider)
            : base(data, userProvider)
        {
        }

        [HttpGet]
        public IHttpActionResult GetNames()
        {
            var hardware = this.Data.HardwareParts.All().Select(d => new {d.Id, d.Name, Cost = d.MoneyCost });
            if (hardware.Any())
            {
                return this.BadRequest("There are no hardware parts to display");
            }

            return this.Ok(hardware);
        }

        [HttpGet]
        public IHttpActionResult GetHardware(int id)
        {
            var hardware = this.Data.HardwareParts.All().Select(d => new { Id = d.Id, Name = d.Name, Cost = d.MoneyCost }).FirstOrDefault(d => d.Id == id);
            if (hardware == null)
            {
                return this.BadRequest("There is no hardware !");
            }

            return this.Ok(hardware);
        }

        [HttpGet]
        public IHttpActionResult GetByUser()
        {
            var hardware =
                this.UserProfile.HardwareParts.Select(
                    d => new {d.Name, Energy = d.CoefficientEnergyBonus, Cost = d.MoneyCost});


            if (hardware.Any())
            {
                return this.BadRequest("Current user has no drinks");
            }

            return this.Ok(hardware);
        }

        [HttpGet]
        public IHttpActionResult BuyHardware(int id)
        {

            var hardware = this.Data.HardwareParts.All().FirstOrDefault(d => d.Id == id);
            if (hardware == null)
            {
                return this.BadRequest("No such hardware exists");
            }

            var hasSuchHardware = this.UserProfile.HardwareParts.FirstOrDefault(d => d.Id == id);
            if (hasSuchHardware != null)
            {
                return this.BadRequest("You already have such a hardware");
            }

            if (this.UserProfile.Money < hardware.MoneyCost)
            {
                return this.BadRequest("This hardware is too expensive for you !");
            }


            this.UserProfile.HardwareParts.Add(hardware);
            this.Data.SaveChanges();

            return this.Ok("You succesfully bought a hardware");
        }

        [HttpGet]
        public IHttpActionResult GetAHardware(int id)
        {
            var hardware = this.UserProfile.HardwareParts.FirstOrDefault(d => d.Id == id);
            if (hardware == null)
            {
                return this.BadRequest("No such hardware exists");
            }
            if (hardware.IsDeleted)
            {
                return this.BadRequest("You have already used that hardware");
            }

            //Drink effect on the user
            this.UserProfile.Energy += hardware.CoefficientEnergyBonus;
            this.UserProfile.Money -= hardware.MoneyCost;

            hardware.IsDeleted = true;
            hardware.DeletedOn = DateTime.Now;

            this.UserProfile.HardwareParts.Add(hardware);
            this.Data.SaveChanges();

            return this.Ok("Congratulations, your energy encreases by " + hardware.CoefficientEnergyBonus);
        }
    }
}
