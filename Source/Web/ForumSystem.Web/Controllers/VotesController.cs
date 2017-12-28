namespace ForumSystem.Web.Controllers
{
    using Microsoft.AspNet.Identity;
    using System.Linq;
    using System.Web.Mvc;

    using ForumSystem.Data.Common.Repository;
    using ForumSystem.Data.Models;

    [Authorize]
    public class VotesController : Controller
    {
        private readonly IRepository<PostVote> _votes;

        public VotesController(IRepository<PostVote> votes)
        {
            this._votes = votes;
        }

        [HttpPost]
        public ActionResult Vote(int postId, int voteType)
        {
            if (voteType > 1)
            {
                voteType = 1;
            }

            if (voteType < -1)
            {
                voteType = -1;
            }

            var userId = this.User.Identity.GetUserId();

            var vote = this._votes
                .All()
                .FirstOrDefault(x => x.AuthorId == userId && x.PostId == postId);

            if (vote == null)
            {
                vote = new PostVote
                {
                    AuthorId = userId,
                    PostId = postId,
                    Type = (VoteType)voteType
                };

                this._votes.Add(vote);
            }
            else
            {
                vote.Type = (VoteType)voteType;
            }

            this._votes.SaveChanges();

            var postVotes = this._votes.All().Where(x => x.PostId == postId).Sum(x => (int)x.Type);

            return this.Json(new { Count = postVotes });
        }
    }
}