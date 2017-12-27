using AutoMapper.QueryableExtensions;
using ForumSystem.Web.ViewModels.PageableFeedbackList;

namespace ForumSystem.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Mvc;

    using ForumSystem.Data.Common.Repository;
    using ForumSystem.Data.Models;

    [Authorize]
    public class PageableFeedbackListController : Controller
    {
        private const int ItemsPerPage = 4;
        private readonly IDeletableEntityRepository<Feedback> _feedbacks;

        public PageableFeedbackListController(IDeletableEntityRepository<Feedback> feedbacks)
        {
            this._feedbacks = feedbacks;
        }

        [HttpGet]
        public ActionResult Index(int id = 1)
        {
            PageableFeedbackListViewModel viewModel;
            if (this.HttpContext.Cache[$"Feedback_page_{id}"] != null)
            {
                viewModel = (PageableFeedbackListViewModel)this.HttpContext.Cache[$"Feedback_page_{id}"];
            }
            else
            {
                var page = id;
                var allItemsCount = this._feedbacks.All().Count();
                var totalPages = (int)Math.Ceiling(allItemsCount / (decimal)ItemsPerPage);
                var itemsToSkip = (page - 1) * ItemsPerPage;
                var feedbacks = this._feedbacks
                    .All()
                    .OrderBy(x => x.CreatedOn)
                    .ThenBy(x => x.Id)
                    .Skip(itemsToSkip)
                    .Take(ItemsPerPage)
                    .Project()
                    .To<FeedbackViewModel>()
                    .ToList();

                viewModel = new PageableFeedbackListViewModel
                {
                    CurrentPage = page,
                    TotalPages = totalPages,
                    Feedbacks = feedbacks
                };

                this.HttpContext.Cache[$"Feedback_page_{id}"] = viewModel;
            }

            return this.View(viewModel);
        }
    }
}