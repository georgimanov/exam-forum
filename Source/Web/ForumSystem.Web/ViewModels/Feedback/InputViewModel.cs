namespace ForumSystem.Web.ViewModels.Feedback
{
    using System.ComponentModel.DataAnnotations;

    public class InputViewModel
    {
        [Required]
        public string Title { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Content { get; set; }
    }
}