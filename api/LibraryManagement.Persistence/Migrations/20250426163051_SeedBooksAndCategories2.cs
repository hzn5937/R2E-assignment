using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LibraryManagement.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SeedBooksAndCategories2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                table: "Books",
                columns: new[] { "Id", "Author", "AvailableQuantity", "CategoryId", "DeletedAt", "Title", "TotalQuantity" },
                values: new object[,]
                {
                    { 1, "Harper Lee", 7, 1, null, "To Kill a Mockingbird", 10 },
                    { 2, "George Orwell", 10, 1, null, "1984", 12 },
                    { 3, "F. Scott Fitzgerald", 5, 1, null, "The Great Gatsby", 8 },
                    { 4, "Jane Austen", 7, 1, null, "Pride and Prejudice", 7 },
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 41);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 42);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 43);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 44);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 45);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 46);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 47);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 48);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 49);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 50);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 6);
        }
    }
}
