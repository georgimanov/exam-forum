namespace ForumSystem.Web.Controllers
{
    using System.Web.Mvc;

    public class FeedbackController : Controller
    {
        [HttpGet]
        public ActionResult Create()
        {
            return this.View();
        }
    }
}