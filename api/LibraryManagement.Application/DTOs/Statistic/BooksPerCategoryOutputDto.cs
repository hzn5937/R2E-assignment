namespace LibraryManagement.Application.DTOs.Statistic
{
    public class BooksPerCategoryOutputDto
    {
        public List<BooksPerCategory> BooksPerCategory { get; set; } = new List<BooksPerCategory>();
    }

    public class BooksPerCategory
    {
        public string CategoryName { get; set; }
        public int BookCount { get; set; }
    }
}
