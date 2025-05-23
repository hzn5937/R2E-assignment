﻿namespace LibraryManagement.Application.DTOs.Request
{
    public class RequestDetailOutputDto
    {
        public int Id { get; set; }
        public List<BookInformation> Books { get; set; }
        public string Requestor { get; set; }
        public string? Approver { get; set; }
        public string Status { get; set; }
        public DateTime RequestedDate { get; set; }
        // Reduntdant, but for consistency and simplicity in frontend
        public string? DateRequested { get; set; }
        public string? DateReturned { get; set; }
    }

    public class BookInformation
    {
        public int? Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string CategoryName { get; set; }
        public string Category { get; set; }
    }
}
