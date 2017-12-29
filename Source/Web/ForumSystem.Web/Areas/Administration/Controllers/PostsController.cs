namespace ForumSystem.Web.Areas.Administration.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using Microsoft.AspNet.Identity;

    using AutoMapper.QueryableExtensions;
    using Kendo;
    using Kendo.Mvc;
    using Kendo.Mvc.Extensions;
    using Kendo.Mvc.UI;

    using ForumSystem.Data.Common.Repository;
    using ForumSystem.Data.Models;
    using ForumSystem.Web.Areas.Administration.ViewModels;
    using ForumSystem.Web.Infrastructure;

    public class PostsController : Controller
    {
        private readonly IDeletableEntityRepository<Post> _posts;
        private readonly ISanitizer _sanitizer;

        public PostsController(IDeletableEntityRepository<Post> posts, ISanitizer sanitizer)
        {
            _posts = posts;
            _sanitizer = sanitizer;

        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Read([DataSourceRequest]DataSourceRequest request, PostsViewModel model)
        {
            var commentsViewModel = this._posts
                .All()
                .Project()
                .To<PostsViewModel>()
                .ToList()
                .ToDataSourceResult(request);

            return this.Json(commentsViewModel, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Create([DataSourceRequest]DataSourceRequest request, PostsViewModel model)
        {
            if (this.ModelState.IsValid && model != null)
            {
                var post = new Post
                {
                    Title = model.Title,
                    Content = this._sanitizer.Sanitize(model.Content),
                };

                if (this.User.Identity.IsAuthenticated)
                {
                    post.AuthorId = this.User.Identity.GetUserId();
                }

                this._posts.Add(post);
                this._posts.SaveChanges();

                return Json(new[] {post}.ToDataSourceResult(request, this.ModelState));
            }

            return this.GetGridOperations(request, model);
        }

        [HttpPost]
        public ActionResult Update([DataSourceRequest]DataSourceRequest request, PostsViewModel model)
        {
            if (this.ModelState.IsValid && model != null)
            {
                var post = _posts.GetById(model.Id);
                post.Title = model.Title;
                post.Content = model.Content;
                post.ModifiedOn = DateTime.UtcNow;

                if (this.User.Identity.IsAuthenticated)
                {
                    post.AuthorId = this.User.Identity.GetUserId();
                }

                _posts.Update(post);
                _posts.SaveChanges();
            }

            return this.GetGridOperations(request, model);
        }

        [HttpPost]
        public ActionResult Destroy([DataSourceRequest]DataSourceRequest request, PostsViewModel model)
        {
            this._posts.Delete(model.Id);
            this._posts.SaveChanges();

            return this.GetGridOperations(request, model);
        }

        protected JsonResult GetGridOperations([DataSourceRequest]DataSourceRequest request, PostsViewModel model)
        {
            return this.Json(new[] { model }.ToDataSourceResult(request, this.ModelState));
        }
    }
}