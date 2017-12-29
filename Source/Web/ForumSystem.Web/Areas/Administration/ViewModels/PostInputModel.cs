namespace ForumSystem.Web.Areas.Administration.ViewModels
{
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    public class PostInputModel
    {
        public int Id { get; set; }

        [MaxLength(100)]
        public string Title { get; set; }

        [AllowHtml]
        //[UIHint("tinymce_full")]
        public string Content { get; set; }
    }
}