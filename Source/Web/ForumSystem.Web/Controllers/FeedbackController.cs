namespace ForumSystem.Web.Controllers
{
    using System.Web.Mvc;
    using Microsoft.AspNet.Identity;

    using ForumSystem.Data.Common.Repository;
    using ForumSystem.Data.Models;
    using ForumSystem.Web.ViewModels.Feedback;

    public class FeedbackController : Controller
    {
        private readonly IDeletableEntityRepository<Feedback> _feedbacks;

        public FeedbackController(IDeletableEntityRepository<Feedback> feedbacks)
        {
            this._feedbacks = feedbacks;
        }

        [HttpGet]
        public ActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(InputViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            var feedback = new Feedback
            {
                Content = model.Content,
                Title = model.Title
            };

            if (this.User.Identity.IsAuthenticated)
            {
                feedback.AuthorId = this.User.Identity.GetUserId();
            }

            this._feedbacks.Add(feedback);
            this._feedbacks.SaveChanges();

            this.TempData["Notification"] = "Feedback created!";

            return this.Redirect("/");
        }
    }
}