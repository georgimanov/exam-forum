namespace ForumSystem.Data.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;
    using System.Linq;

    using ForumSystem.Data.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            this.AutomaticMigrationsEnabled = true;

            // TODO: Remove in production
            this.AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(ApplicationDbContext context)
        {
            if (!context.Feedbacks.Any())
            {
                for (int i = 1; i < 18; i++)
                {
                    var feedback = new Feedback
                    {
                        Title = $"Title {i}",
                        Content = $"Feedback <b>content</b> {i}"
                    };
                    context.Feedbacks.Add(feedback);
                }

                context.SaveChanges();
            }

            if (!context.Tags.Any())
            {
                var tagList = new List<Tag>();
                for (int i = 0; i < 20; i++)
                {
                    var tag = new Tag {Name = $"Tag {i}"};
                    context.Tags.Add(tag);
                    tagList.Add(tag);
                }

                var tagIndex = 0;
                for (int i = 0; i < 20; i++ )
                {
                    var post = new Post { Content = $"Post content {i}", Title = $"Title {i}" };
                    context.Posts.Add(post);
                    for (int j = 0; j < 5; j++)
                    {
                        post.Tags.Add(tagList[tagIndex % tagList.Count]);
                        tagIndex++;
                    }
                }

                context.SaveChanges();
            }
        }
    }
}
