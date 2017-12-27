namespace ForumSystem.Web.ViewModels.Home
{
    using ForumSystem.Data.Models;
    using ForumSystem.Web.Infrastructure.Mapping;

    public class IndexBlogPostTagViewModel : IMapFrom<Tag>
    {
        public int Id { get; set; } 

        public string Name { get; set; }
    }
}