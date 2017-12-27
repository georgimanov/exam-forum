namespace ForumSystem.Web.ViewModels.PageableFeedbackList
{
    using System.Collections.Generic;

    public class PageableFeedbackListViewModel
    {
        public int CurrentPage { get; set; }

        public int TotalPages { get; set; }

        public IEnumerable<FeedbackViewModel> Feedbacks { get; set; }
    }
}