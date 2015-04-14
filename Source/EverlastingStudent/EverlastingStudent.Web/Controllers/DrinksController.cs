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
    public class DrinksController : BaseApiController
    {
        public DrinksController(IEverlastingStudentData data, IUserProvider userProvider)
            : base(data, userProvider)
        {
        }

        [HttpGet]
        public IHttpActionResult GetNames()
        {
            var drinks = this.Data.Drinks.All().Select(d => new { Id = d.Id, Name = d.Name, Energy = d.EnergyBonus, Cost = d.MoneyCost });
            if (drinks == null)
            {
                return this.BadRequest("There are no drinks to display");
            }

            return this.Ok(drinks);
        }

        [HttpGet]
        public IHttpActionResult GetDrink(int id)
        {
            var drink = this.Data.Drinks.All().Select(d => new { Id = d.Id, Name = d.Name, Energy = d.EnergyBonus, Cost = d.MoneyCost }).FirstOrDefault(d => d.Id == id);
            if (drink == null)
            {
                return this.BadRequest("There is no drink !");
            }

            return this.Ok(drink);
        }

        [HttpGet]
        public IHttpActionResult GetByUser()
        {
            var drinks = this.UserProfile.Drinks.Select(d => new { Name = d.Name, Energy = d.EnergyBonus, Cost = d.MoneyCost }).Any();

            if (drinks == null)
            {
                return BadRequest("Current user has no drinks");
            }

            return this.Ok(drinks);
        }

        [HttpGet]
        public IHttpActionResult BuyADrink(int id)
        {

            var drink = this.Data.Drinks.All().FirstOrDefault(d => d.Id == id);
            if (drink == null)
            {
                return BadRequest("No such drink exists");
            }

            var hasSuchDrink = this.UserProfile.Drinks.FirstOrDefault(d => d.Id == id);
            if (hasSuchDrink != null)
            {
                return BadRequest("You already have such a drink");
            }

            if (this.UserProfile.Money < drink.MoneyCost)
            {
                return BadRequest("This drink is too expensive for you !");
            }


            this.UserProfile.Drinks.Add(drink);
            this.Data.SaveChanges();

            return this.Ok("You succesfully bought a drink");
        }

        [HttpGet]
        public IHttpActionResult GetADrink(int id)
        {
            var drink = this.UserProfile.Drinks.FirstOrDefault(d => d.Id == id);
            if (drink == null)
            {
                return BadRequest("No such drink exists");
            }
            if (drink.IsDeleted == true)
            {
                return this.BadRequest("You have already used that drink");
            }

            //Drink effect on the user
            this.UserProfile.Energy += drink.EnergyBonus;
            this.UserProfile.Money -= drink.MoneyCost;

            drink.IsDeleted = true;
            drink.DeletedOn = DateTime.Now;

            this.UserProfile.Drinks.Add(drink);
            this.Data.SaveChanges();

            return this.Ok("Congratulations, your energy encreases by " + drink.EnergyBonus);
        }
    }
}
