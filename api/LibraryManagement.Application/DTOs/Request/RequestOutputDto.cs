namespace LibraryManagement.Application.DTOs.Request
{
    public class RequestOutputDto
    {
        public int Id { get; set; }
        public string Requestor { get; set; }
        public string? Approver { get; set; }
        public DateTime RequestedDate { get; set; }
        public string Status { get; set; }
    }
}
