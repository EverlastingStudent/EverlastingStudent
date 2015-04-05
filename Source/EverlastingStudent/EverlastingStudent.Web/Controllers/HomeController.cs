namespace EverlastingStudent.Web.Controllers
{
    using System.Web.Mvc;

    using EverlastingStudent.Data;

    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
