using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using EverlastingStudent.Common.Infrastructure;
using EverlastingStudent.Data;

namespace EverlastingStudent.Web.Controllers
{
    [Authorize]
    public class HardwareController : BaseApiController
    {
        public HardwareController(IEverlastingStudentData data, IUserProvider userProvider)
            : base(data, userProvider)
        {
        }

        [HttpGet]
        public IHttpActionResult GetNames()
        {
            var hardware = this.Data.HardwareParts.All();
            if (hardware == null)
            {
                return this.BadRequest("There are no hardware parts to display");
            }

            return this.Ok(hardware);
        }

        [HttpGet]
        public IHttpActionResult GetHardware(int id)
        {
            var hardware = this.Data.HardwareParts.All().FirstOrDefault(d => d.Id == id);
            if (hardware == null)
            {
                return this.BadRequest("There is no hardware !");
            }

            return this.Ok(hardware);
        }

        [HttpGet]
        public IHttpActionResult GetByUser()
        {
            var hardware = this.UserProfile.HardwareParts.Select(d => new { Name = d.Name, Energy = d.CoefficientEnergyBonus, Cost = d.MoneyCost }).Any();

            if (hardware == null)
            {
                return BadRequest("Current user has no drinks");
            }

            return this.Ok(hardware);
        }

        [HttpGet]
        public IHttpActionResult BuyHardware(int id)
        {

            var hardware = this.Data.HardwareParts.All().FirstOrDefault(d => d.Id == id);
            if (hardware == null)
            {
                return BadRequest("No such hardware exists");
            }

            var hasSuchHardware = this.UserProfile.HardwareParts.FirstOrDefault(d => d.Id == id);
            if (hasSuchHardware != null)
            {
                return BadRequest("You already have such a hardware");
            }

            if (this.UserProfile.Money < hardware.MoneyCost)
            {
                return BadRequest("This hardware is too expensive for you !");
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
                return BadRequest("No such hardware exists");
            }
            if (hardware.IsDeleted == true)
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
