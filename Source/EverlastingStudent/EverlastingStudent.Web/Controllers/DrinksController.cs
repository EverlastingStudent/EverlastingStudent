namespace EverlastingStudent.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Http;

    using EverlastingStudent.Common.Infrastructure;
    using EverlastingStudent.Data;

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
            var drinks = this.Data.Drinks.All().Select(d => new {d.Id, d.Name, Energy = d.EnergyBonus, Cost = d.MoneyCost });
            if (drinks.Any())
            {
                return this.BadRequest("There are no drinks to display");
            }

            return this.Ok(drinks);
        }

        [HttpGet]
        public IHttpActionResult GetDrink(int id)
        {
            var drink = this.Data.Drinks
                .All()
                .Select(d => new
                {
                    d.Id, 
                    d.Name, 
                    Energy = d.EnergyBonus, 
                    Cost = d.MoneyCost
                })
                .FirstOrDefault(d => d.Id == id);

            if (drink == null)
            {
                return this.BadRequest("There is no drink !");
            }

            return this.Ok(drink);
        }

        [HttpGet]
        public IHttpActionResult GetByUser()
        {
            var drinks = this.UserProfile.Drinks.Select(d => new {d.Name, Energy = d.EnergyBonus, Cost = d.MoneyCost });

            if (drinks.Any())
            {
                return this.BadRequest("Current user has no drinks");
            }

            return this.Ok(drinks);
        }

        [HttpGet]
        public IHttpActionResult BuyADrink(int id)
        {

            var drink = this.Data.Drinks.All().FirstOrDefault(d => d.Id == id);
            if (drink == null)
            {
                return this.BadRequest("No such drink exists");
            }

            var hasSuchDrink = this.UserProfile.Drinks.FirstOrDefault(d => d.Id == id);
            if (hasSuchDrink != null)
            {
                return this.BadRequest("You already have such a drink");
            }

            if (this.UserProfile.Money < drink.MoneyCost)
            {
                return this.BadRequest("This drink is too expensive for you !");
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
                return this.BadRequest("No such drink exists");
            }
            if (drink.IsDeleted)
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
