
namespace ForumSystem.Web.ViewModels.PageableFeedbackList
{
    using System;

    using AutoMapper;

    using ForumSystem.Data.Models;
    using ForumSystem.Web.Infrastructure;
    using ForumSystem.Web.Infrastructure.Mapping;

    public class FeedbackViewModel : IMapFrom<Feedback>, IHaveCustomMappings
    {
        private ISanitizer sanitizer;

        public FeedbackViewModel()
        {
            this.sanitizer = new HtmlSanitizerAdapter();
        }

        public string Author { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string SanitezContent => sanitizer.Sanitize(this.Content);

        public DateTime CreatedOn { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<Feedback, FeedbackViewModel>()
                .ForMember(x => x.Author, opt => opt.MapFrom(x => x.Author.UserName));
        }
    }
}