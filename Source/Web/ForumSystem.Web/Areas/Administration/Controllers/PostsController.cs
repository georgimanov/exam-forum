using System.Web.Mvc;

using Kendo;
using Kendo.Mvc;

namespace ForumSystem.Web.Areas.Administration.Controllers
{
    public class PostsController : Controller
    {
        // GET: Administration/Posts
        public ActionResult Index()
        {
            return View();
        }
    }
}