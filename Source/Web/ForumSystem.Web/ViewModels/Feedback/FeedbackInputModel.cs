namespace ForumSystem.Web.ViewModels.Feedback
{
    using System.ComponentModel.DataAnnotations;

    public class FeedbackInputModel
    {
        [Required]
        [StringLength(20)]
        public string Title { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Content { get; set; }
    }
}