using System.ComponentModel.DataAnnotations;
namespace _30_TranTheTruong_Assignment01_FrontEnd.Models
{
   
    public class LoginVM
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Email format is invalid.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; } = string.Empty;
    }

    public class LoginResponseVM
    {
        public string Token { get; set; } = string.Empty;
        public short AccountId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }

    public class AccountVM
    {
        public short AccountId { get; set; }

        [Required(ErrorMessage = "Account name is required.")]
        [StringLength(100, ErrorMessage = "Account name must be at most 100 characters.")]
        public string? AccountName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Email format is invalid.")]
        [StringLength(70, ErrorMessage = "Email must be at most 70 characters.")]
        public string? AccountEmail { get; set; }

        [Required(ErrorMessage = "Role is required.")]
        [Range(1, 2, ErrorMessage = "Role must be 1 (Staff) or 2 (Lecturer).")]
        public int? AccountRole { get; set; }

        [StringLength(70, ErrorMessage = "Password must be at most 70 characters.")]
        public string? AccountPassword { get; set; }

        public string RoleName => AccountRole == 1
            ? "Staff"
            : AccountRole == 2
                ? "Lecturer"
                : "-";
    }

    public class CategoryVM
    {
        public short CategoryId { get; set; }

        [Required(ErrorMessage = "Category name is required.")]
        [StringLength(100, ErrorMessage = "Category name must be at most 100 characters.")]
        public string? CategoryName { get; set; }

        [Required(ErrorMessage = "Category description is required.")]
        [StringLength(250, ErrorMessage = "Description must be at most 250 characters.")]
        public string? CategoryDesciption { get; set; }

        public short? ParentCategoryId { get; set; }

        public bool? IsActive { get; set; } = true;
    }

    public class TagVM
    {
        public int TagId { get; set; }
        public string? TagName { get; set; }
        public string? Note { get; set; }
    }

    public class NewsVM
    {
        public string NewsArticleId { get; set; } = string.Empty;
        public string? NewsTitle { get; set; }
        public string? Headline { get; set; }
        public DateTimeOffset? CreatedDate { get; set; }
        public string? NewsContent { get; set; }
        public string? NewsSource { get; set; }
        public short? CategoryId { get; set; }
        public bool? NewsStatus { get; set; }
        public short? CreatedById { get; set; }
        public short? UpdatedById { get; set; }
        public DateTimeOffset? ModifiedDate { get; set; }

        public CategoryVM? Category { get; set; }
        public AccountVM? CreatedBy { get; set; }
        public List<TagVM> Tags { get; set; } = new();
    }

    public class NewsFormVM
    {
        public string? NewsArticleId { get; set; }

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
        public short? CategoryId { get; set; }

        public bool NewsStatus { get; set; } = true;

        public List<int> TagIds { get; set; } = new();

        /// <summary>"Index" or "History" - where to go back after saving.</summary>
        public string? ReturnTo { get; set; }
    }

    public class NewsPageVM
    {
        public List<NewsVM> Items { get; set; } = new();
        public List<CategoryVM> Categories { get; set; } = new();
        public List<TagVM> Tags { get; set; } = new();
        public string? Search { get; set; }
        public bool HistoryOnly { get; set; }
    }

    public class ProfileVM
    {
        public short AccountId { get; set; }

        [Required(ErrorMessage = "Account name is required.")]
        [StringLength(100, ErrorMessage = "Account name must be at most 100 characters.")]
        public string? AccountName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Email format is invalid.")]
        [StringLength(70, ErrorMessage = "Email must be at most 70 characters.")]
        public string? AccountEmail { get; set; }

        [StringLength(70, ErrorMessage = "Password must be at most 70 characters.")]
        [DataType(DataType.Password)]
        public string? NewPassword { get; set; }
    }

    public class ReportVM
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Total { get; set; }
        public int ActiveCount { get; set; }
        public int InactiveCount { get; set; }
        public List<ReportGroupVM> ByCategory { get; set; } = new();
        public List<ReportGroupVM> ByCreator { get; set; } = new();
        public List<ReportItemVM> Articles { get; set; } = new();
    }

    public class ReportGroupVM
    {
        public string Name { get; set; } = string.Empty;
        public int Count { get; set; }
    }

    public class ReportItemVM
    {
        public string NewsArticleId { get; set; } = string.Empty;
        public string? NewsTitle { get; set; }
        public string? Headline { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? CategoryName { get; set; }
        public string? CreatedByName { get; set; }
        public bool NewsStatus { get; set; }
    }
}
