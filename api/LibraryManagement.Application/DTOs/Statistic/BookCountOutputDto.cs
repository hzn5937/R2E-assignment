namespace LibraryManagement.Application.DTOs.Statistic
{
    public class BookCountOutputDto
    {
        public int TotalBooks { get; set; }
        public int TotalAvailable { get; set; }
        public int TotalNotAvailable { get; set; }
    }
}