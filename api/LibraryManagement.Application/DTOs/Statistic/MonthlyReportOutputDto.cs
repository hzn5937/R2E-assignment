namespace LibraryManagement.Application.DTOs.Statistic
{
    public class MonthlyReportOutputDto
    {
        public DateTime Month { get; set; }
        public int TotalRequests { get; set; }
        public int ApprovedRequests { get; set; }
        public int RejectedRequests { get; set; }
        public int PendingRequests { get; set; }
        public List<MonthlyPopularBook> PopularBooks { get; set; } = new List<MonthlyPopularBook>();
        public List<MonthlyPopularCategory> PopularCategories { get; set; } = new List<MonthlyPopularCategory>();
        public int TotalActiveUsers { get; set; }
    }

    public class MonthlyPopularBook
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public int BorrowCount { get; set; }
    }

    public class MonthlyPopularCategory
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int BorrowCount { get; set; }
    }
}