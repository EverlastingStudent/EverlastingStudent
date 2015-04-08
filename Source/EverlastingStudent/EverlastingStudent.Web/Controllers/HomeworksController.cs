namespace EverlastingStudent.Web.Controllers
{
    using System.Web.Mvc;

    public class HomeworksController : BaseApiController
    {
        // GET: Homeworks
        public ActionResult Index()
        {
            return View();
        }
    }
}