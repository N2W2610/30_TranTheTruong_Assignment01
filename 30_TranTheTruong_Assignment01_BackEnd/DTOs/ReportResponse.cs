namespace _30_TranTheTruong_Assignment01_BackEnd.DTOs
{
    public class ReportResponse
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Total { get; set; }
        public int ActiveCount { get; set; }
        public int InactiveCount { get; set; }
        public List<ReportGroup> ByCategory { get; set; } = new();
        public List<ReportGroup> ByCreator { get; set; } = new();
        public List<ReportItem> Articles { get; set; } = new();
    }

    public class ReportGroup
    {
        public string Name { get; set; } = string.Empty;
        public int Count { get; set; }
    }

    public class ReportItem
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
