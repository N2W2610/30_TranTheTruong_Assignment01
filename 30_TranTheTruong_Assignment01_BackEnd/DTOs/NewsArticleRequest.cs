using System.ComponentModel.DataAnnotations;

namespace _30_TranTheTruong_Assignment01_BackEnd.DTOs
{
    public class NewsArticleRequest
    {
        [Required(ErrorMessage = "News title is required.")]
        [StringLength(400, ErrorMessage = "News title must be at most 400 characters.")]
        public string? NewsTitle { get; set; }

        [Required(ErrorMessage = "Headline is required.")]
        [StringLength(150, ErrorMessage = "Headline must be at most 150 characters.")]
        public string? Headline { get; set; }

        [Required(ErrorMessage = "News content is required.")]
        [StringLength(4000, ErrorMessage = "News content must be at most 4000 characters.")]
        public string? NewsContent { get; set; }

        [StringLength(400, ErrorMessage = "News source must be at most 400 characters.")]
        public string? NewsSource { get; set; }

        [Required(ErrorMessage = "Category is required.")]
        public short? CategoryID { get; set; }

        /// <summary>true = active (1), false = inactive (0)</summary>
        public bool NewsStatus { get; set; } = true;

        public List<int> TagIds { get; set; } = new();
    }
}
