namespace LibraryManagement.Application.DTOs.Statistic
{
    public class BookQuantitiesOutputDto
    {
        public int TotalBooks { get; set; }
        public int AvailableBooks { get; set; }
        public int BorrowedBooks { get; set; }
    }
}
