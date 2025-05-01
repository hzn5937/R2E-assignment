namespace LibraryManagement.Application.DTOs.Book
{
    public class BookDetailOutputDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int TotalQuantity { get; set; }
        public int AvailableQuantity { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string CategoryName { get; set; }
        public string Author { get; set; }
    }
}
