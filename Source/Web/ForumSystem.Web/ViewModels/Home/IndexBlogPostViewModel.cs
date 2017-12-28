namespace ForumSystem.Web.ViewModels.Home
{
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;

    using ForumSystem.Data.Models;
    using ForumSystem.Web.Infrastructure.Mapping;

    public class IndexBlogPostViewModel : IMapFrom<Post>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public int VotesCount { get; set; }

        public IEnumerable<IndexBlogPostTagViewModel> Tags { get; set; }

        public string Url => $"questions/{this.Id}/{this.Title.ToLower().Replace(" ", "-")}";

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<Post, IndexBlogPostViewModel>()
                .ForMember(
                    x => x.VotesCount, 
                    opt => opt.MapFrom(x => x.Votes.Any() ? x.Votes.Sum(v => (int) v.Type) : 0));
        }
    }
}