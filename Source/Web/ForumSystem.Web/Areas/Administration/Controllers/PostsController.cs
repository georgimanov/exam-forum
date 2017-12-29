namespace ForumSystem.Web.Areas.Administration.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using Microsoft.AspNet.Identity;

    using AutoMapper.QueryableExtensions;
    using Kendo.Mvc.Extensions;
    using Kendo.Mvc.UI;

    using ForumSystem.Data.Common.Repository;
    using ForumSystem.Data.Models;
    using ForumSystem.Web.Infrastructure;
    using ForumSystem.Web.Areas.Administration.ViewModels;

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
        public ActionResult Create([DataSourceRequest]DataSourceRequest request, PostInputModel model)
        {
            var postId = 0;
            if (this.ModelState.IsValid && model != null)
            {
                var postToAdd = new Post
                {
                    Title = model.Title,
                    Content = this._sanitizer.Sanitize(model.Content),
                };

                if (this.User.Identity.IsAuthenticated)
                {
                    postToAdd.AuthorId = this.User.Identity.GetUserId();
                }

                this._posts.Add(postToAdd);
                this._posts.SaveChanges();

                postId = postToAdd.Id;
            }

            var postToDisplay = this._posts.All().Project().To<PostsViewModel>().First(x => x.Id == postId);

            return Json(new[] { postToDisplay }.ToDataSourceResult(request, this.ModelState));
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

                this._posts.Update(post);
                this._posts.SaveChanges();
            }

            var postToDisplay = this._posts.All().Project().To<PostsViewModel>().First(x => x.Id == model.Id);

            return Json(new[] { postToDisplay }.ToDataSourceResult(request, this.ModelState));
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