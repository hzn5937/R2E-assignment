using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LibraryManagement.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Role = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    RefreshToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RefreshTokenExpiryTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Author = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TotalQuantity = table.Column<int>(type: "int", nullable: false),
                    AvailableQuantity = table.Column<int>(type: "int", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CategoryId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Books_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "BookBorrowingRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequestorId = table.Column<int>(type: "int", nullable: false),
                    ApproverId = table.Column<int>(type: "int", nullable: true),
                    DateRequested = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookBorrowingRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookBorrowingRequests_Users_ApproverId",
                        column: x => x.ApproverId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BookBorrowingRequests_Users_RequestorId",
                        column: x => x.RequestorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BookBorrowingRequestDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequestId = table.Column<int>(type: "int", nullable: false),
                    BookId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookBorrowingRequestDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookBorrowingRequestDetails_BookBorrowingRequests_RequestId",
                        column: x => x.RequestId,
                        principalTable: "BookBorrowingRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookBorrowingRequestDetails_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "Author", "AvailableQuantity", "CategoryId", "DeletedAt", "Title", "TotalQuantity" },
                values: new object[,]
                {
                    { 41, "Tom Holland", 6, null, null, "Rubicon: The Last Years of the Roman Republic", 7 },
                    { 47, "Ron Chernow", 8, null, null, "Alexander Hamilton", 10 },
                    { 50, "Gayle Laakmann McDowell", 15, null, null, "Cracking the Coding Interview", 20 }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Fiction" },
                    { 2, "Science" },
                    { 3, "History" },
                    { 4, "Fantasy" },
                    { 5, "Biography" },
                    { 6, "Technology" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "PasswordHash", "RefreshToken", "RefreshTokenExpiryTime", "Role", "Username" },
                values: new object[,]
                {
                    { 1, "admin1@example.com", "AQAAAAIAAYagAAAAEPTT95ZoG4A1YLWx/ZNulKJnxjPRCHF0YTDIsrWxWmRP758ZKIXwdJeSlJmMUBdBNg==", null, null, "Admin", "admin1" },
                    { 2, "admin2@example.com", "AQAAAAIAAYagAAAAENU2b+si1vmTif26kOAaQRMGUII5FhFFsfXTj+4Q0EpyKfr3N5sMfyVdPtvXgdAaxw==", null, null, "Admin", "admin2" },
                    { 3, "admin3@example.com", "AQAAAAIAAYagAAAAEGcCqumrEjHVfWf7fvN4k3Ay8wrKe+HGQeLKLTLcoQBhg8HpByBxmnfrfn1EI+y1/w==", null, null, "Admin", "admin3" },
                    { 4, "user1@example.com", "AQAAAAIAAYagAAAAEIUwCm938Qh/1WSiFC7ErKwWnDRIxHoLapxEsciuLwR69HmvmO0N0r+956rlB7aYiw==", null, null, "User", "user1" },
                    { 5, "user2@example.com", "AQAAAAIAAYagAAAAEGN+8nQU5cTdsBc14AvnaPa0lcmShqWEibpFiu7DuLyidQZ9rUQIeZZKeeSFt6D8Nw==", null, null, "User", "user2" },
                    { 6, "user3@example.com", "AQAAAAIAAYagAAAAEHcZ+9aopLOUH9JJYeBY0NFrtz6jG0fRhkKGCnUCPBlSJpxYWQuXzVlCaA8XeCBnAA==", null, null, "User", "user3" },
                    { 7, "user4@example.com", "AQAAAAIAAYagAAAAEDTB1D5b/VBWdMCgvHBDKavUXWLUifGYGxKvctESoamJb3OgfGPoTRlC45CwgXniCw==", null, null, "User", "user4" },
                    { 8, "user5@example.com", "AQAAAAIAAYagAAAAEGeTHbnBhb/wg208idw/LeLTieyIPsbd+XuVB64omUXrVfR5l4J2tHywl7hlr7X/Fg==", null, null, "User", "user5" },
                    { 9, "user6@example.com", "AQAAAAIAAYagAAAAEG8Yh9n4YfMWgPAGCslqe/3F02k/co4bSVfBfLxBWKZ5MiT05utpDgOft5L9MTaodA==", null, null, "User", "user6" },
                    { 10, "user7@example.com", "AQAAAAIAAYagAAAAEMeEtFyNMhWR2xj12Mas+X+jJvQJZunTTEjT+EszzlgGuNokNmGe7TWbUeFH+7QGlQ==", null, null, "User", "user7" },
                    { 11, "user8@example.com", "AQAAAAIAAYagAAAAEAfZb2Nm5E9F78qj/i+H261pOrDd5n+GmNzX1MGSJOxaAmLWK1KKj6SWdHuOUFZa3Q==", null, null, "User", "user8" }
                });

            migrationBuilder.InsertData(
                table: "BookBorrowingRequests",
                columns: new[] { "Id", "ApproverId", "DateRequested", "RequestorId", "Status" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2025, 4, 23, 18, 32, 24, 253, DateTimeKind.Utc).AddTicks(4794), 4, "Approved" },
                    { 2, null, new DateTime(2025, 4, 28, 18, 32, 24, 253, DateTimeKind.Utc).AddTicks(4829), 5, "Waiting" },
                    { 3, 2, new DateTime(2025, 5, 1, 18, 32, 24, 253, DateTimeKind.Utc).AddTicks(4831), 6, "Rejected" },
                    { 4, null, new DateTime(2025, 5, 2, 18, 32, 24, 253, DateTimeKind.Utc).AddTicks(4833), 4, "Waiting" },
                    { 5, 1, new DateTime(2025, 4, 18, 18, 32, 24, 253, DateTimeKind.Utc).AddTicks(4834), 7, "Approved" },
                    { 6, 3, new DateTime(2025, 4, 25, 18, 32, 24, 253, DateTimeKind.Utc).AddTicks(4837), 8, "Approved" },
                    { 7, null, new DateTime(2025, 4, 30, 18, 32, 24, 253, DateTimeKind.Utc).AddTicks(4839), 9, "Waiting" },
                    { 8, null, new DateTime(2025, 5, 3, 6, 32, 24, 253, DateTimeKind.Utc).AddTicks(4840), 4, "Waiting" },
                    { 9, 1, new DateTime(2025, 4, 29, 18, 32, 24, 253, DateTimeKind.Utc).AddTicks(4858), 5, "Approved" },
                    { 10, null, new DateTime(2025, 5, 2, 23, 20, 24, 253, DateTimeKind.Utc).AddTicks(4861), 5, "Waiting" },
                    { 11, 2, new DateTime(2025, 4, 27, 18, 32, 24, 253, DateTimeKind.Utc).AddTicks(4863), 6, "Rejected" },
                    { 12, null, new DateTime(2025, 5, 2, 6, 32, 24, 253, DateTimeKind.Utc).AddTicks(4865), 6, "Waiting" },
                    { 13, 3, new DateTime(2025, 5, 2, 13, 44, 24, 253, DateTimeKind.Utc).AddTicks(4868), 6, "Approved" },
                    { 14, null, new DateTime(2025, 4, 30, 6, 32, 24, 253, DateTimeKind.Utc).AddTicks(4869), 10, "Waiting" },
                    { 15, 1, new DateTime(2025, 4, 30, 13, 44, 24, 253, DateTimeKind.Utc).AddTicks(4871), 10, "Approved" },
                    { 16, 2, new DateTime(2025, 4, 30, 23, 20, 24, 253, DateTimeKind.Utc).AddTicks(4874), 10, "Approved" },
                    { 17, null, new DateTime(2025, 5, 1, 13, 44, 24, 253, DateTimeKind.Utc).AddTicks(4875), 11, "Waiting" },
                    { 18, 3, new DateTime(2025, 5, 1, 23, 20, 24, 253, DateTimeKind.Utc).AddTicks(4878), 11, "Rejected" }
                });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "Author", "AvailableQuantity", "CategoryId", "DeletedAt", "Title", "TotalQuantity" },
                values: new object[,]
                {
                    { 1, "Harper Lee", 7, 1, null, "To Kill a Mockingbird", 10 },
                    { 2, "George Orwell", 10, 1, null, "1984", 12 },
                    { 3, "F. Scott Fitzgerald", 5, 1, null, "The Great Gatsby", 8 },
                    { 4, "Jane Austen", 7, 1, null, "Pride and Prejudice", 10 },
                    { 5, "J.D. Salinger", 3, 1, null, "The Catcher in the Rye", 6 },
                    { 6, "Yuval Noah Harari", 11, 3, null, "Sapiens: A Brief History of Humankind", 15 },
                    { 7, "Stephen Hawking", 8, 2, null, "A Brief History of Time", 9 },
                    { 8, "Carl Sagan", 6, 2, null, "Cosmos", 7 },
                    { 9, "Richard Dawkins", 5, 2, null, "The Selfish Gene", 5 },
                    { 10, "Jared Diamond", 9, 3, null, "Guns, Germs, and Steel", 10 },
                    { 11, "Howard Zinn", 4, 3, null, "A People's History of the United States", 7 },
                    { 12, "J.R.R. Tolkien", 18, 4, null, "The Hobbit", 20 },
                    { 13, "J.K. Rowling", 22, 4, null, "Harry Potter and the Sorcerer's Stone", 25 },
                    { 14, "George R.R. Martin", 15, 4, null, "A Game of Thrones", 18 },
                    { 15, "Patrick Rothfuss", 10, 4, null, "The Name of the Wind", 14 },
                    { 16, "Brandon Sanderson", 16, 4, null, "Mistborn: The Final Empire", 16 },
                    { 17, "Walter Isaacson", 9, 5, null, "Steve Jobs", 11 },
                    { 18, "Michelle Obama", 13, 5, null, "Becoming", 13 },
                    { 19, "Laura Hillenbrand", 7, 5, null, "Unbroken", 9 },
                    { 20, "Tara Westover", 8, 5, null, "Educated", 10 },
                    { 21, "Anne Frank", 8, 5, null, "The Diary of a Young Girl", 8 },
                    { 22, "Robert C. Martin", 12, 6, null, "Clean Code: A Handbook of Agile Software Craftsmanship", 15 },
                    { 23, "Andrew Hunt, David Thomas", 11, 6, null, "The Pragmatic Programmer: Your Journey to Mastery", 12 },
                    { 24, "Steve McConnell", 9, 6, null, "Code Complete", 10 },
                    { 25, "Erich Gamma, Richard Helm, Ralph Johnson, John Vlissides", 6, 6, null, "Design Patterns: Elements of Reusable Object-Oriented Software", 9 },
                    { 26, "Thomas H. Cormen, Charles E. Leiserson, Ronald L. Rivest, Clifford Stein", 5, 6, null, "Introduction to Algorithms", 7 },
                    { 27, "Aldous Huxley", 10, 1, null, "Brave New World", 11 },
                    { 28, "William Golding", 7, 1, null, "Lord of the Flies", 9 },
                    { 29, "Ray Bradbury", 11, 1, null, "Fahrenheit 451", 13 },
                    { 30, "Douglas Adams", 14, 1, null, "The Hitchhiker's Guide to the Galaxy", 15 },
                    { 31, "Herman Melville", 6, 1, null, "Moby Dick", 6 },
                    { 32, "Leo Tolstoy", 3, 1, null, "War and Peace", 5 },
                    { 33, "Charles Darwin", 7, 2, null, "The Origin of Species", 8 },
                    { 34, "James D. Watson", 5, 2, null, "The Double Helix", 6 },
                    { 35, "Rachel Carson", 7, 2, null, "Silent Spring", 7 },
                    { 36, "Rebecca Skloot", 8, 2, null, "The Immortal Life of Henrietta Lacks", 10 },
                    { 37, "Thomas S. Kuhn", 4, 2, null, "The Structure of Scientific Revolutions", 5 },
                    { 38, "David McCullough", 9, 3, null, "1776", 9 },
                    { 39, "William L. Shirer", 5, 3, null, "The Rise and Fall of the Third Reich", 6 },
                    { 40, "Mary Beard", 7, 3, null, "SPQR: A History of Ancient Rome", 8 },
                    { 42, "Barbara W. Tuchman", 6, 3, null, "The Guns of August", 6 },
                    { 43, "J.R.R. Tolkien", 17, 4, null, "The Fellowship of the Ring", 19 },
                    { 44, "C.S. Lewis", 13, 4, null, "The Chronicles of Narnia", 15 },
                    { 45, "Neil Gaiman", 10, 4, null, "American Gods", 12 },
                    { 46, "Terry Pratchett", 14, 4, null, "The Color of Magic", 14 },
                    { 48, "Doris Kearns Goodwin", 7, 5, null, "Team of Rivals: The Political Genius of Abraham Lincoln", 8 },
                    { 49, "Walter Isaacson", 10, 6, null, "The Innovators: How a Group of Hackers, Geniuses, and Geeks Created the Digital Revolution", 11 }
                });

            migrationBuilder.InsertData(
                table: "BookBorrowingRequestDetails",
                columns: new[] { "Id", "BookId", "RequestId" },
                values: new object[,]
                {
                    { 1, 1, 1 },
                    { 2, 7, 1 },
                    { 3, 12, 1 },
                    { 4, 2, 2 },
                    { 5, 13, 2 },
                    { 6, 22, 2 },
                    { 7, 33, 2 },
                    { 8, 47, 2 },
                    { 9, 6, 3 },
                    { 10, 18, 3 },
                    { 11, 4, 4 },
                    { 12, 14, 4 },
                    { 13, 26, 4 },
                    { 14, 40, 4 },
                    { 15, 50, 5 },
                    { 16, 17, 6 },
                    { 17, 23, 6 },
                    { 18, 3, 7 },
                    { 19, 10, 7 },
                    { 20, 43, 7 },
                    { 21, 8, 8 },
                    { 22, 27, 8 },
                    { 23, 16, 9 },
                    { 24, 36, 9 },
                    { 25, 44, 9 },
                    { 26, 30, 10 },
                    { 27, 5, 11 },
                    { 28, 32, 11 },
                    { 29, 28, 12 },
                    { 30, 29, 12 },
                    { 31, 31, 12 },
                    { 32, 46, 13 },
                    { 33, 9, 14 },
                    { 34, 24, 14 },
                    { 35, 34, 14 },
                    { 36, 38, 14 },
                    { 37, 42, 14 },
                    { 38, 19, 15 },
                    { 39, 20, 15 },
                    { 40, 49, 16 },
                    { 41, 35, 17 },
                    { 42, 45, 17 },
                    { 43, 25, 17 },
                    { 44, 41, 18 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookBorrowingRequestDetails_BookId",
                table: "BookBorrowingRequestDetails",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_BookBorrowingRequestDetails_RequestId_BookId",
                table: "BookBorrowingRequestDetails",
                columns: new[] { "RequestId", "BookId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BookBorrowingRequests_ApproverId",
                table: "BookBorrowingRequests",
                column: "ApproverId");

            migrationBuilder.CreateIndex(
                name: "IX_BookBorrowingRequests_RequestorId",
                table: "BookBorrowingRequests",
                column: "RequestorId");

            migrationBuilder.CreateIndex(
                name: "IX_Books_CategoryId",
                table: "Books",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true,
                filter: "[Email] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookBorrowingRequestDetails");

            migrationBuilder.DropTable(
                name: "BookBorrowingRequests");

            migrationBuilder.DropTable(
                name: "Books");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
